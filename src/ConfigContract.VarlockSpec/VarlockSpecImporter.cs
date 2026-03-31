using System.Text.RegularExpressions;
using ConfigContract.Abstractions;

namespace ConfigContract.VarlockSpec;

public static partial class VarlockSpecImporter
{
  public static VarlockSpecImportResult ImportFromFile(string filePath)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

    var text = File.ReadAllText(filePath);
    var contractName = Path.GetFileNameWithoutExtension(filePath);
    return ImportFromText(contractName, text, filePath);
  }

  public static VarlockSpecImportResult ImportFromText(string contractName, string text, string? filePath = null)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(contractName);
    ArgumentNullException.ThrowIfNull(text);

    var diagnostics = new List<ContractDiagnostic>();
    var fields = new List<ContractField>();

    var lines = text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
    for (var index = 0; index < lines.Length; index++)
    {
      var rawLine = lines[index];
      var trimmedLine = rawLine.Trim();
      if (string.IsNullOrWhiteSpace(trimmedLine)
        || trimmedLine is "/**" or "*/")
      {
        continue;
      }

      if (TryGetCommentDecorator(trimmedLine, out var rootDecorator))
      {
        ProcessCommentLine(rootDecorator, filePath, index + 1, diagnostics);
        continue;
      }

      if (trimmedLine.StartsWith('#') || trimmedLine.StartsWith('*'))
      {
        continue;
      }

      var separatorIndex = trimmedLine.IndexOf('=');
      if (separatorIndex < 0)
      {
        diagnostics.Add(CreateDiagnostic(
          "VSP0001",
          $"Line '{trimmedLine}' is not a supported config item declaration.",
          filePath,
          index + 1));
        continue;
      }

      var key = trimmedLine[..separatorIndex].Trim();
      var rhs = trimmedLine[(separatorIndex + 1)..].Trim();
      if (string.IsNullOrWhiteSpace(key))
      {
        diagnostics.Add(CreateDiagnostic(
          "VSP0002",
          "Config item key cannot be empty.",
          filePath,
          index + 1));
        continue;
      }

      var tokens = SupportedTokenRegex().Matches(rhs).Select(static match => match.Value).ToArray();
      if (tokens.Length == 0)
      {
        diagnostics.Add(CreateDiagnostic(
          "VSP0003",
          $"Config item '{key}' does not contain a supported Varlock type token.",
          filePath,
          index + 1));
        continue;
      }

      var unsupportedRightHandSide = SupportedTokenRegex().Replace(rhs, string.Empty).Trim();
      if (!string.IsNullOrWhiteSpace(unsupportedRightHandSide))
      {
        diagnostics.Add(CreateDiagnostic(
          "VSP0005",
          $"Config item '{key}' contains unsupported right-hand-side content '{unsupportedRightHandSide}'.",
          filePath,
          index + 1));
        continue;
      }

      var fieldDiagnostics = new List<ContractDiagnostic>();
      ContractValueKind? kind = null;
      var required = false;
      var sensitive = false;
      string? defaultValue = null;

      foreach (var token in tokens)
      {
        if (TryMapTypeDecorator(token, out var mappedKind))
        {
          if (kind is not null)
          {
            fieldDiagnostics.Add(CreateDiagnostic(
              "VSP0006",
              $"Config item '{key}' declares multiple supported type decorators. Only one type decorator is supported per field.",
              filePath,
              index + 1));
            continue;
          }

          kind = mappedKind;
          continue;
        }

        switch (token)
        {
          case "@required":
            required = true;
            break;
          case "@sensitive":
            sensitive = true;
            break;
          default:
            if (token.StartsWith("@default(", StringComparison.Ordinal))
            {
              if (defaultValue is not null)
              {
                fieldDiagnostics.Add(CreateDiagnostic(
                  "VSP0007",
                  $"Config item '{key}' declares multiple @default decorators. Only one default is supported per field.",
                  filePath,
                  index + 1));
                continue;
              }

              defaultValue = token[9..^1].Trim().Trim('"');
            }
            else
            {
              fieldDiagnostics.Add(CreateDiagnostic(
                "VSP1001",
                $"Field decorator '{token}' is not supported in the current Varlock compatibility lane.",
                filePath,
                index + 1));
            }

            break;
        }
      }

      if (fieldDiagnostics.Count > 0)
      {
        diagnostics.AddRange(fieldDiagnostics);
        continue;
      }

      if (kind is null)
      {
        diagnostics.Add(CreateDiagnostic(
          "VSP0004",
          $"Config item '{key}' is missing a supported type decorator such as @string, @integer, or @boolean.",
          filePath,
          index + 1));
        continue;
      }

      fields.Add(new ContractField(key, kind.Value, required, sensitive, defaultValue));
    }

    return new VarlockSpecImportResult(new ContractDescriptor(contractName, fields), diagnostics);
  }

  private static void ProcessCommentLine(
    string decorator,
    string? filePath,
    int lineNumber,
    List<ContractDiagnostic> diagnostics)
  {
    var decoratorName = GetDecoratorName(decorator);

    if (string.Equals(decorator, "@env-spec", StringComparison.Ordinal))
    {
      return;
    }

    if (string.Equals(decoratorName, "@plugin", StringComparison.Ordinal))
    {
      diagnostics.Add(CreateDiagnostic(
        "VSP2001",
        "Root decorator @plugin is outside the first-wave Varlock compatibility surface for ConfigContract.",
        filePath,
        lineNumber));

      return;
    }

    diagnostics.Add(CreateDiagnostic(
      "VSP2002",
      $"Root decorator '{decoratorName}' is not supported in the current Varlock compatibility lane.",
      filePath,
      lineNumber));
  }

  private static bool TryGetCommentDecorator(string trimmedLine, out string decorator)
  {
    decorator = string.Empty;

    if (trimmedLine.StartsWith('#'))
    {
      var candidate = trimmedLine[1..].Trim();
      if (candidate.StartsWith('@'))
      {
        decorator = candidate;
        return true;
      }

      return false;
    }

    if (!trimmedLine.StartsWith('*'))
    {
      return false;
    }

    var blockCandidate = trimmedLine[1..].Trim();
    if (!blockCandidate.StartsWith('@'))
    {
      return false;
    }

    decorator = blockCandidate;
    return true;
  }

  private static bool TryMapTypeDecorator(string token, out ContractValueKind kind)
  {
    switch (token)
    {
      case "@string":
        kind = ContractValueKind.String;
        return true;
      case "@integer":
        kind = ContractValueKind.Integer;
        return true;
      case "@boolean":
        kind = ContractValueKind.Boolean;
        return true;
      default:
        kind = default;
        return false;
    }
  }

  private static string GetDecoratorName(string decorator)
  {
    var endIndex = decorator.Length;
    foreach (var delimiter in new[] { '(', ' ', '\t' })
    {
      var delimiterIndex = decorator.IndexOf(delimiter);
      if (delimiterIndex >= 0 && delimiterIndex < endIndex)
      {
        endIndex = delimiterIndex;
      }
    }

    return endIndex == decorator.Length ? decorator : decorator[..endIndex];
  }

  private static ContractDiagnostic CreateDiagnostic(
    string code,
    string message,
    string? filePath,
    int lineNumber)
  {
    return new ContractDiagnostic(
      code,
      ContractDiagnosticSeverity.Error,
      message,
      new ContractSourceLocation(filePath, lineNumber, 1));
  }

  [GeneratedRegex("@\\w+(?:\\([^)]*\\))?")]
  private static partial Regex SupportedTokenRegex();
}
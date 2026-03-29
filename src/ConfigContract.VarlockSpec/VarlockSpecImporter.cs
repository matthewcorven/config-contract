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
        || trimmedLine is "/**" or "*/"
        || trimmedLine.StartsWith('*'))
      {
        continue;
      }

      if (trimmedLine.StartsWith('#'))
      {
        ProcessCommentLine(trimmedLine, filePath, index + 1, diagnostics);
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

      ContractValueKind? kind = null;
      var required = false;
      var sensitive = false;
      string? defaultValue = null;

      foreach (var token in tokens)
      {
        switch (token)
        {
          case "@string":
            kind = ContractValueKind.String;
            break;
          case "@integer":
            kind = ContractValueKind.Integer;
            break;
          case "@boolean":
            kind = ContractValueKind.Boolean;
            break;
          case "@required":
            required = true;
            break;
          case "@sensitive":
            sensitive = true;
            break;
          default:
            if (token.StartsWith("@default(", StringComparison.Ordinal))
            {
              defaultValue = token[9..^1].Trim().Trim('"');
            }
            else
            {
              diagnostics.Add(CreateDiagnostic(
                "VSP1001",
                $"Field decorator '{token}' is not supported in the current Varlock compatibility lane.",
                filePath,
                index + 1));
            }

            break;
        }
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
    string trimmedLine,
    string? filePath,
    int lineNumber,
    List<ContractDiagnostic> diagnostics)
  {
    if (trimmedLine.StartsWith("# @plugin", StringComparison.Ordinal))
    {
      diagnostics.Add(CreateDiagnostic(
        "VSP2001",
        "Root decorator @plugin is outside the first-wave Varlock compatibility surface for ConfigContract.",
        filePath,
        lineNumber));
    }
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
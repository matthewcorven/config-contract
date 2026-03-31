using ConfigContract.Abstractions;
using ConfigContract.VarlockSpec;

namespace ConfigContract.Tests;

public sealed class VarlockSpecImporterTests
{
  [Fact]
  public void ImportFromFileReadsSupportedFixture()
  {
    var fixturePath = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Varlock", "basic.env.schema");

    var result = VarlockSpecImporter.ImportFromFile(fixturePath);

    Assert.True(result.IsSuccessful);
    Assert.Empty(result.Diagnostics);
    Assert.Equal("basic.env", result.Descriptor.Name);
    Assert.Collection(
      result.Descriptor.Fields,
      field =>
      {
        Assert.Equal("APP_NAME", field.Name);
        Assert.Equal(ContractValueKind.String, field.Kind);
        Assert.Equal("config-contract", field.DefaultValue);
        Assert.False(field.Required);
        Assert.False(field.Sensitive);
      },
      field =>
      {
        Assert.Equal("APP_PORT", field.Name);
        Assert.Equal(ContractValueKind.Integer, field.Kind);
        Assert.Equal("5000", field.DefaultValue);
        Assert.False(field.Required);
        Assert.False(field.Sensitive);
      },
      field =>
      {
        Assert.Equal("DATABASE_URL", field.Name);
        Assert.Equal(ContractValueKind.String, field.Kind);
        Assert.True(field.Required);
        Assert.False(field.Sensitive);
        Assert.Null(field.DefaultValue);
      },
      field =>
      {
        Assert.Equal("API_KEY", field.Name);
        Assert.Equal(ContractValueKind.String, field.Kind);
        Assert.False(field.Required);
        Assert.True(field.Sensitive);
        Assert.Null(field.DefaultValue);
      });
  }

  [Fact]
  public void ImportFromFileReportsUnsupportedRootDecorator()
  {
    var fixturePath = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Varlock", "unsupported-plugin.env.schema");

    var result = VarlockSpecImporter.ImportFromFile(fixturePath);

    Assert.False(result.IsSuccessful);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("VSP2001", diagnostic.Code);
    var field = Assert.Single(result.Descriptor.Fields);
    Assert.Equal("APP_NAME", field.Name);
    Assert.Equal(ContractValueKind.String, field.Kind);
    Assert.Equal("config-contract", field.DefaultValue);
  }

  [Fact]
  public void ImportFromTextAcceptsHashStyleEnvSpecMarkerAndTrimsQuotedDefault()
  {
    var text = """
# @env-spec
FEATURE_ENABLED = @boolean @default(  "true"  )
""";

    var result = VarlockSpecImporter.ImportFromText("supported-boolean", text, "supported-boolean.env.schema");

    Assert.True(result.IsSuccessful);
    Assert.Empty(result.Diagnostics);
    var field = Assert.Single(result.Descriptor.Fields);
    Assert.Equal("FEATURE_ENABLED", field.Name);
    Assert.Equal(ContractValueKind.Boolean, field.Kind);
    Assert.Equal("true", field.DefaultValue);
  }

  [Fact]
  public void ImportFromTextReportsUnsupportedRightHandSideAndSkipsLossyField()
  {
    const string filePath = "unsupported-rhs.env.schema";
    var text = "API_KEY=@string @sensitive\nLEGACY_VALUE=@string ???";

    var result = VarlockSpecImporter.ImportFromText("unsupported-rhs", text, filePath);

    Assert.False(result.IsSuccessful);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("VSP0005", diagnostic.Code);
    Assert.Equal(filePath, diagnostic.Location?.FilePath);
    Assert.Equal(2, diagnostic.Location?.Line);
    var field = Assert.Single(result.Descriptor.Fields);
    Assert.Equal("API_KEY", field.Name);
    Assert.DoesNotContain(result.Descriptor.Fields, candidate => candidate.Name == "LEGACY_VALUE");
  }

  [Fact]
  public void ImportFromTextReportsRemainingDocumentedDiagnosticCodesWithSourceLocations()
  {
    const string filePath = "diagnostic-matrix.env.schema";
    var text = """
NOT_A_DECLARATION
= @string
MISSING_TYPE_TOKEN = plain-text
MISSING_SUPPORTED_TYPE = @required
UNSUPPORTED_DECORATOR = @string @readonly
""";

    var result = VarlockSpecImporter.ImportFromText("diagnostic-matrix", text, filePath);

    Assert.False(result.IsSuccessful);
    Assert.Empty(result.Descriptor.Fields);
    Assert.Collection(
      result.Diagnostics,
      diagnostic =>
      {
        Assert.Equal("VSP0001", diagnostic.Code);
        Assert.Equal(ContractDiagnosticSeverity.Error, diagnostic.Severity);
        Assert.Equal(filePath, diagnostic.Location?.FilePath);
        Assert.Equal(1, diagnostic.Location?.Line);
      },
      diagnostic =>
      {
        Assert.Equal("VSP0002", diagnostic.Code);
        Assert.Equal(ContractDiagnosticSeverity.Error, diagnostic.Severity);
        Assert.Equal(filePath, diagnostic.Location?.FilePath);
        Assert.Equal(2, diagnostic.Location?.Line);
      },
      diagnostic =>
      {
        Assert.Equal("VSP0003", diagnostic.Code);
        Assert.Equal(ContractDiagnosticSeverity.Error, diagnostic.Severity);
        Assert.Equal(filePath, diagnostic.Location?.FilePath);
        Assert.Equal(3, diagnostic.Location?.Line);
      },
      diagnostic =>
      {
        Assert.Equal("VSP0004", diagnostic.Code);
        Assert.Equal(ContractDiagnosticSeverity.Error, diagnostic.Severity);
        Assert.Equal(filePath, diagnostic.Location?.FilePath);
        Assert.Equal(4, diagnostic.Location?.Line);
      },
      diagnostic =>
      {
        Assert.Equal("VSP1001", diagnostic.Code);
        Assert.Equal(ContractDiagnosticSeverity.Error, diagnostic.Severity);
        Assert.Contains("@readonly", diagnostic.Message, StringComparison.Ordinal);
        Assert.Equal(filePath, diagnostic.Location?.FilePath);
        Assert.Equal(5, diagnostic.Location?.Line);
      });
  }

  [Fact]
  public void ImportFromTextReportsConflictingSupportedTypeDecorators()
  {
    var result = VarlockSpecImporter.ImportFromText("conflicting-types", "PORT=@string @integer");

    Assert.False(result.IsSuccessful);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("VSP0006", diagnostic.Code);
    Assert.Empty(result.Descriptor.Fields);
  }

  [Fact]
  public void ImportFromTextReportsMultipleDefaults()
  {
    var result = VarlockSpecImporter.ImportFromText("multiple-defaults", "PORT=@string @default(\"8080\") @default(\"9090\")");

    Assert.False(result.IsSuccessful);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("VSP0007", diagnostic.Code);
    Assert.Empty(result.Descriptor.Fields);
  }

  [Fact]
  public void ImportFromTextReportsGenericUnsupportedBlockCommentRootDecorator()
  {
    var text = """
/**
 * @custom-root(example)
 */
API_KEY=@string
""";

    var result = VarlockSpecImporter.ImportFromText("unsupported-root", text, "unsupported-root.env.schema");

    Assert.False(result.IsSuccessful);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("VSP2002", diagnostic.Code);
    Assert.Contains("@custom-root", diagnostic.Message, StringComparison.Ordinal);
    Assert.Equal(2, diagnostic.Location?.Line);
    var field = Assert.Single(result.Descriptor.Fields);
    Assert.Equal("API_KEY", field.Name);
  }

  [Fact]
  public void ImportFromTextRejectsPrefixedRootDecoratorVariants()
  {
    var text = """
# @env-spec-legacy
# @pluginized(example)
API_KEY=@string
""";

    var result = VarlockSpecImporter.ImportFromText("prefixed-root", text, "prefixed-root.env.schema");

    Assert.False(result.IsSuccessful);
    Assert.Collection(
      result.Diagnostics,
      diagnostic =>
      {
        Assert.Equal("VSP2002", diagnostic.Code);
        Assert.Contains("@env-spec-legacy", diagnostic.Message, StringComparison.Ordinal);
        Assert.Equal(1, diagnostic.Location?.Line);
      },
      diagnostic =>
      {
        Assert.Equal("VSP2002", diagnostic.Code);
        Assert.Contains("@pluginized", diagnostic.Message, StringComparison.Ordinal);
        Assert.Equal(2, diagnostic.Location?.Line);
      });

    var field = Assert.Single(result.Descriptor.Fields);
    Assert.Equal("API_KEY", field.Name);
  }
}
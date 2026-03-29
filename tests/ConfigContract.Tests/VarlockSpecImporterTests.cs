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
    Assert.Equal(4, result.Descriptor.Fields.Count);
    Assert.Contains(result.Descriptor.Fields, field => field.Name == "DATABASE_URL" && field.Required);
    Assert.Contains(result.Descriptor.Fields, field => field.Name == "API_KEY" && field.Sensitive);
  }

  [Fact]
  public void ImportFromFileReportsUnsupportedRootDecorator()
  {
    var fixturePath = Path.Combine(AppContext.BaseDirectory, "Fixtures", "Varlock", "unsupported-plugin.env.schema");

    var result = VarlockSpecImporter.ImportFromFile(fixturePath);

    Assert.False(result.IsSuccessful);
    var diagnostic = Assert.Single(result.Diagnostics);
    Assert.Equal("VSP2001", diagnostic.Code);
  }
}
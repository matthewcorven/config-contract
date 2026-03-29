namespace ConfigContract.Abstractions;

public sealed record ContractDescriptor
{
	public ContractDescriptor(string name)
		: this(name, Array.Empty<ContractField>())
	{
	}

	public ContractDescriptor(string name, IReadOnlyList<ContractField> fields)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(name);
		ArgumentNullException.ThrowIfNull(fields);

		Name = name;
		Fields = fields;
	}

	public string Name { get; init; }

	public IReadOnlyList<ContractField> Fields { get; init; }
}
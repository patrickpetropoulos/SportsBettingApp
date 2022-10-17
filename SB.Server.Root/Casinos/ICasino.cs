using SB.Server.Root.Base;

namespace SB.Server.Root.Casinos;

public interface ICasino : IEntity
{
	Guid Id { get; set; }
	string Name { get; set; }
	string CountryCode { get; set; }
}
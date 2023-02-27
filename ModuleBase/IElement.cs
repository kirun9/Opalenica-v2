namespace Opalenica.ModuleBase;

public interface IElement
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public bool IsSelected { get; }
}
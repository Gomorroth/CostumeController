namespace gomoru.su.CostumeController
{
    public interface IParameterNamesProvider
    {
        void GetParameterNames(ref ValueList<(string Group, string Name)> list);
    }
}

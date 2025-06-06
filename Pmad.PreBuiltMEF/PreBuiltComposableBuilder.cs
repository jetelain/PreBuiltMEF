namespace Pmad.PreBuiltMEF
{
    internal class PreBuiltComposableBuilder<TPart> : PreBuiltPartBuilderBase<TPart> where TPart : class
    {
        public IPreBuiltComposableDefinition Build()
        {
            return new PreBuiltComposableDefinition<TPart> (exportDefinitions, importDefinitions);
        }
    }
}

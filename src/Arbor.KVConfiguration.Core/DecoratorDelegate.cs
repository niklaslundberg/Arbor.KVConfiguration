namespace Arbor.KVConfiguration.Core
{
    public class DecoratorDelegate : AppSettingsDecoratorBuilder
    {
        public DecoratorDelegate(AppSettingsBuilder appSettingsBuild) : base(appSettingsBuild, new NullDecorator())
        {
            
        }
    }
}

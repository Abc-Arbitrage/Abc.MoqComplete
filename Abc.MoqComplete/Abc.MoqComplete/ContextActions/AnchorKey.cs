using JetBrains.Util;

namespace Abc.MoqComplete.ContextActions
{
    public static class AnchorKey
    {
        public static readonly Key<object> FillWithMockContextActionKey = new Key<object>(nameof(FillWithMockContextActionKey));
        public static readonly Key<object> FillParamWithMockContextActionKey = new Key<object>(nameof(FillParamWithMockContextActionKey));
    }
}

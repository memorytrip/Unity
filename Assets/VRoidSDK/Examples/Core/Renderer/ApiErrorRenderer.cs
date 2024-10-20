using Pixiv.VroidSdk.Api.DataModel;
using Pixiv.VroidSdk.Logger;
using VRoidSDK.Examples.Core.View;
using VRoidSDK.Examples.Core.Model;

namespace VRoidSDK.Examples.Core.Renderer
{
    public class ApiErrorRenderer : IRenderer
    {
        private ApiErrorFormat _errorFormat;

        public ApiErrorRenderer(ApplicationModel model)
        {
            _errorFormat = model.ApiError;
            SdkLogger.LogError(_errorFormat);
        }

        public void Rendering(RootView root)
        {
            root.ApiErrorMessage.Active = true;
            root.ApiErrorMessage.Text = _errorFormat.message;
        }
    }
}

using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Messaging;
using Mde.Project.Mobile.Messages;
using Mde.Project.Mobile.ViewModels.Base;

namespace Mde.Project.Mobile.Controls.Base
{
    public abstract class SfContentPage : ContentPage
    {
        protected abstract Type ViewModelType { get; set; }
        public SfContentPage()
        {
            WeakReferenceMessenger.Default.Register<SendToastrMessage>(this, async (r, m) =>
            {
                // Check if the message's target type matches the ViewModelType or is null
                if (m.TargetType == null || m.TargetType == ViewModelType)
                {
                    await OnToastrMessage(m.Message);
                }
            });
        }

        public async Task OnToastrMessage(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(message, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}

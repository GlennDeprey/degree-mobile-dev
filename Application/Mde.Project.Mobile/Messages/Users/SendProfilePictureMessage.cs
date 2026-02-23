namespace Mde.Project.Mobile.Messages.Users
{
    public class SendProfilePictureMessage
    {
        public ImageSource NewImage { get; set; }

        public SendProfilePictureMessage(ImageSource newImage)
        {
            NewImage = newImage;
        }
    }
}

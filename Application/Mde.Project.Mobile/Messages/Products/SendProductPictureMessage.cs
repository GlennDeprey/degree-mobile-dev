namespace Mde.Project.Mobile.Messages.Products
{
    public class SendProductPictureMessage
    {
        public ImageSource NewImage { get; set; }

        public SendProductPictureMessage(ImageSource newImage)
        {
            NewImage = newImage;
        }
    }
}

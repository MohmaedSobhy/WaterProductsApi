namespace WaterProducts.services.emial
{
    public interface IEmailSender
    {
        public Task<bool> SendEmailAsync();
    }
}

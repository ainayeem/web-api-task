
namespace StudentManagement.API.BGServices
{
    public class EmailService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                //
                Console.WriteLine($"service is running at: {DateTimeOffset.Now}");

                //
                Console.WriteLine($"Email sent successfully {DateTimeOffset.Now}");


                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); //worker after 5 sec
            }
        }
    }
}

namespace Twitch.Containers
{
    public class Emote
    {
        public int Id { get; }
        private string BaseUrl = "http://static-cdn.jtvnw.net/emoticons/v1/<emote ID>/<size>";

        public Emote(int id)
        {
            Id = id;
        }
    }
}

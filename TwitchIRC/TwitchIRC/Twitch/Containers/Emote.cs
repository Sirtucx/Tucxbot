namespace Twitch.Containers
{
    public class Emote
    {
        public int ID { get; protected set; }
        private string BaseURL = "http://static-cdn.jtvnw.net/emoticons/v1/<emote ID>/<size>";

        public Emote(int id)
        {
            ID = id;
        }
    }
}

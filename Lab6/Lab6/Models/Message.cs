namespace Lab6.Models;

public class Message
{
    public int Id { get; set; }

    public string SenderName { get; set; } = "";

    public string MessageBody { get; set; } = "";

    public string? RoomName { get; set; }

    public byte MessageType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
namespace API.DTOs.Bookings
{
    public class BookingDurationDto
    {
        public Guid RoomGuid { get; set; }
        public String RoomName { get; set; }
        public TimeSpan BookingLenght { get; set; }
    }
}

namespace Stations.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Dto.Export;
    using Models.Enums;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportDelayedTrains(StationsDbContext context, string dateAsString)
        {
            DateTime date = DateTime.ParseExact(dateAsString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            TrainDto[] trains = context
                .Trains
                .Where(t => t.Trips.Any(tr => tr.Status == TripStatus.Delayed && tr.DepartureTime <= date))
                .Select(t => new TrainDto()
                {
                    TrainNumber = t.TrainNumber,
                    DelayedTimes = t.Trips.Count(tr => tr.Status == TripStatus.Delayed && tr.DepartureTime <= date),
                    MaxDelayedTime = t.Trips.Max(tr => tr.TimeDifference)
                })
                .OrderByDescending(t => t.DelayedTimes)
                .ThenByDescending(t => t.MaxDelayedTime)
                .ThenBy(t => t.TrainNumber)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(trains, Formatting.Indented);
            return jsonString;
        }

        public static string ExportCardsTicket(StationsDbContext context, string cardType)
        {
            CardType type = Enum.Parse<CardType>(cardType);

            CardDto[] cards = context
                .Cards
                .Where(c => c.Type == type && c.BoughtTickets.Count != 0)
                .Select(c => new CardDto()
                {
                    Name = c.Name,
                    CardType = c.Type,
                    Tickets = c.BoughtTickets
                        .Select(bt => new TicketDto()
                        {
                            OriginDestination = bt.Trip.OriginStation.Name,
                            DestinationStation = bt.Trip.DestinationStation.Name,
                            DepartureTime = bt.Trip.DepartureTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                        })
                        .ToArray()
                })
                .OrderBy(c => c.Name)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(CardDto[]), new XmlRootAttribute("Cards"));
            StringBuilder sb = new StringBuilder();
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cards, xmlNamespaces);

            return sb.ToString().Trim();
        }
    }
}
namespace Stations.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using Dto.Import;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enums;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportStations(StationsDbContext context, string jsonString)
		{
		    StationDto[] stationDtos = JsonConvert.DeserializeObject<StationDto[]>(jsonString);
		    List<Station> stations = new List<Station>();
		    StringBuilder sb = new StringBuilder();

		    foreach (StationDto dto in stationDtos)
		    {
		        if (!IsValid(dto))
		        {
		            sb.AppendLine(FailureMessage);
                    continue;
		        }

		        if (stations.Any(s => s.Name == dto.Name))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        if (dto.Town == null)
		        {
		            dto.Town = dto.Name;
		        }

		        Station station = Mapper.Map<Station>(dto);
		        stations.Add(station);
		        sb.AppendLine(string.Format(SuccessMessage, dto.Name));
		    }

		    context.Stations.AddRange(stations);
		    context.SaveChanges();

		    return sb.ToString().Trim();
		}

		public static string ImportClasses(StationsDbContext context, string jsonString)
		{
		    SeatingClassDto[] classDtos = JsonConvert.DeserializeObject<SeatingClassDto[]>(jsonString);
		    List<SeatingClass> seatingClasses = new List<SeatingClass>();
		    StringBuilder sb = new StringBuilder();

		    foreach (SeatingClassDto dto in classDtos)
		    {
		        if (!IsValid(dto))
		        {
		            sb.AppendLine(FailureMessage);
                    continue;
		        }

		        if (seatingClasses.Any(sc => sc.Name == dto.Name) || seatingClasses.Any(sc => sc.Abbreviation == dto.Abbreviation))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
                }

		        SeatingClass seatingClass = Mapper.Map<SeatingClass>(dto);
		        seatingClasses.Add(seatingClass);
		        sb.AppendLine(string.Format(SuccessMessage, seatingClass.Name));
		    }

		    context.SeatingClasses.AddRange(seatingClasses);
		    context.SaveChanges();

		    return sb.ToString().Trim();
		}

		public static string ImportTrains(StationsDbContext context, string jsonString)
		{
		    TrainDto[] trainDtos = JsonConvert.DeserializeObject<TrainDto[]>(jsonString, new JsonSerializerSettings()
		    {
		        NullValueHandling = NullValueHandling.Ignore
		    });

		    StringBuilder sb = new StringBuilder();
		    List<Train> trains = new List<Train>();

		    foreach (TrainDto dto in trainDtos)
		    {
		        if (!IsValid(dto))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        if (trains.Any(t => t.TrainNumber == dto.TrainNumber))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        if (!dto.Seats.All(IsValid))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        if (!dto.Seats.All(s => context.SeatingClasses.Any(sc => sc.Name == s.Name && sc.Abbreviation == s.Abbreviation)))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        TrainType type = Enum.Parse<TrainType>(dto.Type);

		        TrainSeat[] trainSeats = dto.Seats.Select(s => new TrainSeat
		            {
		                SeatingClass = context.SeatingClasses.SingleOrDefault(sc => sc.Name == s.Name && sc.Abbreviation == s.Abbreviation),
		                Quantity = s.Quantity.Value
		            })
		            .ToArray();

		        Train train = new Train
		        {
		            TrainNumber = dto.TrainNumber,
		            Type = type,
		            TrainSeats = trainSeats
		        };

		        trains.Add(train);
		        sb.AppendLine(string.Format(SuccessMessage, dto.TrainNumber));
		    }

		    context.Trains.AddRange(trains);
		    context.SaveChanges();

		    return sb.ToString().Trim();
		}

		public static string ImportTrips(StationsDbContext context, string jsonString)
		{
		    TripDto[] tripDtos = JsonConvert.DeserializeObject<TripDto[]>(jsonString);
		    StringBuilder sb = new StringBuilder();
		    List<Trip> trips = new List<Trip>();

		    foreach (TripDto dto in tripDtos)
		    {
		        if (!IsValid(dto))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        Train train = context.Trains.SingleOrDefault(t => t.TrainNumber == dto.Train);
		        Station originStation = context.Stations.SingleOrDefault(s => s.Name == dto.OriginStation);
		        Station destinationStation = context.Stations.SingleOrDefault(s => s.Name == dto.DestinationStation);

		        if (train == null || originStation == null || destinationStation == null)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        DateTime departureTime = DateTime.ParseExact(dto.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
		        DateTime arrivalTime = DateTime.ParseExact(dto.ArrivalTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
		        TimeSpan timeDifference;

		        if (dto.TimeDifference != null)
		        {
		            timeDifference = TimeSpan.ParseExact(dto.TimeDifference, @"hh\:mm", CultureInfo.InvariantCulture);
		        }

		        if (departureTime > arrivalTime)
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        TripStatus status = Enum.Parse<TripStatus>(dto.Status);

		        Trip trip = new Trip
		        {
		            Train = train,
		            OriginStation = originStation,
		            DestinationStation = destinationStation,
		            DepartureTime = departureTime,
		            ArrivalTime = arrivalTime,
		            Status = status,
		            TimeDifference = timeDifference
		        };

		        trips.Add(trip);
		        sb.AppendLine($"Trip from {dto.OriginStation} to {dto.DestinationStation} imported.");
		    }

		    context.Trips.AddRange(trips);
		    context.SaveChanges();

            return sb.ToString().Trim();
		}

		public static string ImportCards(StationsDbContext context, string xmlString)
		{
		    XmlSerializer serializer = new XmlSerializer(typeof(CardDto[]), new XmlRootAttribute("Cards"));
		    CardDto[] cardDtos = (CardDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();
		    List<CustomerCard> cards = new List<CustomerCard>();

		    foreach (CardDto dto in cardDtos)
		    {
		        if (!IsValid(dto))
		        {
		            sb.AppendLine(FailureMessage);
		            continue;
		        }

		        CardType cardType = Enum.TryParse<CardType>(dto.CardType, out var card) ? card : CardType.Normal;

		        CustomerCard customerCard = new CustomerCard
		        {
		            Name = dto.Name,
		            Type = cardType,
		            Age = dto.Age
		        };

		        cards.Add(customerCard);
		        sb.AppendLine(string.Format(SuccessMessage, customerCard.Name));
		    }

		    context.Cards.AddRange(cards);
		    context.SaveChanges();

		    return sb.ToString().Trim();
		}

		public static string ImportTickets(StationsDbContext context, string xmlString)
		{
            XmlSerializer serializer = new XmlSerializer(typeof(TicketDto[]), new XmlRootAttribute("Tickets"));
            TicketDto[] ticketDtos = (TicketDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();
            List<Ticket> tickets = new List<Ticket>();

            foreach (TicketDto dto in ticketDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                DateTime departureTime = DateTime.ParseExact(dto.Trip.DepartureTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                Trip trip = context.Trips
                    .Include(t => t.OriginStation)
                    .Include(t => t.DestinationStation)
                    .Include(t => t.Train)
                    .ThenInclude(t => t.TrainSeats)
                    .SingleOrDefault(t => t.OriginStation.Name == dto.Trip.OriginStation &&
                                                              t.DestinationStation.Name == dto.Trip.DestinationStation &&
                                                              t.DepartureTime == departureTime);
                if (trip == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                CustomerCard card = null;
                if (dto.Card != null)
                {
                    card = context.Cards.SingleOrDefault(c => c.Name == dto.Card.Name);

                    if (card == null)
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }
                }

                string seatingClassAbbreviation = dto.Seat.Substring(0, 2);
                int quantity = int.Parse(dto.Seat.Substring(2));

                TrainSeat seatExists = trip.Train.TrainSeats.SingleOrDefault(s => s.SeatingClass.Abbreviation == seatingClassAbbreviation && quantity <= s.Quantity);
                if (seatExists == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                string seat = dto.Seat;

                Ticket ticket = new Ticket
                {
                    Trip = trip,
                    CustomerCard = card,
                    Price = dto.Price,
                    SeatingPlace = seat
                };

                tickets.Add(ticket);
                sb.AppendLine(string.Format("Ticket from {0} to {1} departing at {2} imported.",
                    trip.OriginStation.Name,
                    trip.DestinationStation.Name,
                    trip.DepartureTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)));
            }

            context.Tickets.AddRange(tickets);
            context.SaveChanges();

		    return sb.ToString().Trim();
		}

	    public static bool IsValid(object obj)
	    {
	        ValidationContext validationContext = new ValidationContext(obj);
	        List<ValidationResult> validationResults = new List<ValidationResult>();

	        return Validator.TryValidateObject(obj, validationContext, validationResults, true);
	    }
	}
}
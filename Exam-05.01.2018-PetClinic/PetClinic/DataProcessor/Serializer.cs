namespace PetClinic.DataProcessor
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
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            AnimalExportDto[] animals = context
                .Animals
                .Where(a => a.Passport.OwnerPhoneNumber == phoneNumber)
                .Select(a => new AnimalExportDto()
                {
                    OwnerName = a.Passport.OwnerName,
                    AnimalName = a.Name,
                    Age = a.Age,
                    SerialNumber = a.PassportSerialNumber,
                    RegisteredOn = a.Passport.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                })
                .OrderBy(a => a.Age)
                .ThenBy(a => a.SerialNumber)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(animals, Formatting.Indented);
            return jsonString;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            ProcedureExportDto[] procedures = context
                .Procedures
                .Select(p => new ProcedureExportDto()
                {
                    Passport = p.Animal.PassportSerialNumber,
                    OwnerNumber = p.Animal.Passport.OwnerPhoneNumber,
                    DateTime = p.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    AnimalAids = p.ProcedureAnimalAids
                        .Select(paa => new AnimalAidExportDto()
                        {
                            Name = paa.AnimalAid.Name,
                            Price = paa.AnimalAid.Price,
                        })
                        .ToArray(),
                    TotalPrice = p.ProcedureAnimalAids.Select(paa => paa.AnimalAid.Price).Sum()
                })
                .OrderBy(p => DateTime.ParseExact(p.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .ThenBy(p => p.Passport)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(ProcedureExportDto[]), new XmlRootAttribute("Procedures"));
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), procedures, xmlNamespaces);

            return sb.ToString().Trim();
        }
    }
}
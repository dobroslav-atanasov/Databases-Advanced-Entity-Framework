namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Dto.Import;
    using Models;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string SuccessMessage = "Record {0} successfully imported.";
        private const string SuccessMessageAnimal = "Record {0} Passport №: {1} successfully imported.";
        private const string SuccessMessageProcedure = "Record successfully imported.";
        private const string ErrorMessage = "Error: Invalid data.";

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            AnimalAidDto[] animalAidDtos = JsonConvert.DeserializeObject<AnimalAidDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<AnimalAid> animalAids = new List<AnimalAid>();

            foreach (AnimalAidDto dto in animalAidDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (animalAids.Any(aa => aa.Name == dto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                AnimalAid animalAid = new AnimalAid()
                {
                    Name = dto.Name,
                    Price = dto.Price
                };

                animalAids.Add(animalAid);
                sb.AppendLine(string.Format(SuccessMessage, dto.Name));
            }

            context.AnimalAids.AddRange(animalAids);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            AnimalDto[] animalDtos = JsonConvert.DeserializeObject<AnimalDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Animal> animals = new List<Animal>();

            foreach (AnimalDto dto in animalDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!IsValid(dto.Passport))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime dateTime;
                bool isValidDate = DateTime.TryParseExact(dto.Passport.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                if (!isValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (context.Passports.Any(p => p.SerialNumber == dto.Passport.SerialNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Passport passport = new Passport()
                {
                    SerialNumber = dto.Passport.SerialNumber,
                    OwnerName = dto.Passport.OwnerName,
                    OwnerPhoneNumber = dto.Passport.OwnerPhoneNumber,
                    RegistrationDate = dateTime
                };

                context.Passports.Add(passport);
                context.SaveChanges();

                Animal animal = new Animal()
                {
                    Name = dto.Name,
                    Type = dto.Type,
                    Age = dto.Age,
                    PassportSerialNumber = dto.Passport.SerialNumber
                };

                animals.Add(animal);
                sb.AppendLine(string.Format(SuccessMessageAnimal, dto.Name, dto.Passport.SerialNumber));
            }

            context.Animals.AddRange(animals);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(VetDto[]), new XmlRootAttribute("Vets"));
            VetDto[] vetDtos = (VetDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();
            List<Vet> vets = new List<Vet>();

            foreach (VetDto dto in vetDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (vets.Any(v => v.PhoneNumber == dto.PhoneNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Vet vet = new Vet()
                {
                    Name = dto.Name,
                    Profession = dto.Profession,
                    Age = dto.Age,
                    PhoneNumber = dto.PhoneNumber
                };

                vets.Add(vet);
                sb.AppendLine(string.Format(SuccessMessage, dto.Name));
            }

            context.AddRange(vets);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));
            ProcedureDto[] procedureDtos = (ProcedureDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();

            foreach (ProcedureDto dto in procedureDtos)
            {
                if (!context.Vets.Any(v => v.Name == dto.Vet))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!context.Animals.Any(a => a.Passport.SerialNumber == dto.Animal))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime dateTime;
                bool isValidDate = DateTime.TryParseExact(dto.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                if (!isValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                bool isValidAnimalAids = true;
                foreach (AnimalAidProcedureDto dtoAnimalAid in dto.AnimalAids)
                {
                    if (!context.AnimalAids.Any(aa => aa.Name == dtoAnimalAid.Name))
                    {
                        isValidAnimalAids = false;
                        break;
                    }
                }

                if (!isValidAnimalAids)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isExistAnimalAid = false;
                List<string> animalAids = new List<string>();
                foreach (AnimalAidProcedureDto dtoAnimalAid in dto.AnimalAids)
                {
                    if (animalAids.Contains(dtoAnimalAid.Name))
                    {
                        isExistAnimalAid = true;
                        break;
                    }
                    animalAids.Add(dtoAnimalAid.Name);
                }

                if (isExistAnimalAid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Procedure procedure = new Procedure()
                {
                    VetId = context.Vets.SingleOrDefault(v => v.Name == dto.Vet).Id,
                    AnimalId = context.Animals.SingleOrDefault(a => a.Passport.SerialNumber == dto.Animal).Id,
                    DateTime = dateTime
                };

                context.Procedures.Add(procedure);
                context.SaveChanges();

                sb.AppendLine(SuccessMessageProcedure);

                List<ProcedureAnimalAid> procedureAnimalAids = new List<ProcedureAnimalAid>();
                foreach (AnimalAidProcedureDto dtoAnimalAid in dto.AnimalAids)
                {
                    ProcedureAnimalAid procedureAnimalAid = new ProcedureAnimalAid()
                    {
                        Procedure = procedure,
                        AnimalAidId = context.AnimalAids.SingleOrDefault(aa => aa.Name == dtoAnimalAid.Name).Id
                    };

                    procedureAnimalAids.Add(procedureAnimalAid);
                }

                context.ProceduresAnimalAids.AddRange(procedureAnimalAids);
                context.SaveChanges();
            }

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
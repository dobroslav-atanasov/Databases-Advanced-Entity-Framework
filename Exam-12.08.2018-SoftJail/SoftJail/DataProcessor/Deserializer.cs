namespace SoftJail.DataProcessor
{

    using Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string InvalidData = "Invalid Data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            DepartmentDto[] departmentDtos = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();

            foreach (DepartmentDto dto in departmentDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                bool isValidCells = true;
                foreach (CellDto dtoCell in dto.Cells)
                {
                    if (!IsValid(dtoCell))
                    {
                        isValidCells = false;
                        break;
                    }
                }

                if (!isValidCells)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                Department department = new Department()
                {
                    Name = dto.Name
                };

                context.Departments.Add(department);
                context.SaveChanges();

                List<Cell> cells = new List<Cell>();
                foreach (CellDto dtoCell in dto.Cells)
                {
                    Cell cell = new Cell()
                    {
                        CellNumber = dtoCell.CellNumber,
                        HasWindow = dtoCell.HasWindow,
                        DepartmentId = department.Id
                    };

                    cells.Add(cell);
                }

                context.Cells.AddRange(cells);
                context.SaveChanges();

                sb.AppendLine($"Imported {dto.Name} with {dto.Cells.Length} cells");
            }

            return sb.ToString().Trim();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            PrisonerDto[] prisonerDtos = JsonConvert.DeserializeObject<PrisonerDto[]>(jsonString, settings);
            StringBuilder sb = new StringBuilder();

            foreach (PrisonerDto dto in prisonerDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                DateTime incarceration;
                if (!DateTime.TryParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out incarceration))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                DateTime release;
                if (dto.ReleaseDate != null)
                {
                    if (!DateTime.TryParseExact(dto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out release))
                    {
                        sb.AppendLine(InvalidData);
                        continue;
                    }
                }



                bool isValidEmails = true;
                foreach (MailDto dtoMail in dto.Mails)
                {
                    if (!IsValid(dtoMail))
                    {
                        isValidEmails = false;
                        break;
                    }
                }

                if (!isValidEmails)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                Prisoner prisoner = new Prisoner()
                {
                    FullName = dto.FullName,
                    Nickname = dto.Nickname,
                    Age = dto.Age,
                    IncarcerationDate = incarceration,
                    ReleaseDate = dto.ReleaseDate == null ? (DateTime?) null : DateTime.ParseExact(dto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    Bail = dto.Bail,
                    CellId = dto.CellId
                };

                context.Prisoners.Add(prisoner);
                context.SaveChanges();

                List<Mail> mails = new List<Mail>();
                foreach (MailDto dtoMail in dto.Mails)
                {
                    Mail mail = new Mail()
                    {
                        Description = dtoMail.Description,
                        Sender = dtoMail.Sender,
                        Address = dtoMail.Address,
                        PrisonerId = prisoner.Id
                    };

                    mails.Add(mail);
                }

                context.Mails.AddRange(mails);
                context.SaveChanges();

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            return sb.ToString().Trim();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OfficerDto[]), new XmlRootAttribute("Officers"));
            OfficerDto[] officerDtos = (OfficerDto[])serializer.Deserialize(new StringReader(xmlString));
            StringBuilder sb = new StringBuilder();

            foreach (OfficerDto dto in officerDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                Position position;
                if (!Enum.TryParse<Position>(dto.Position, out position))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                Weapon weapon;
                if (!Enum.TryParse<Weapon>(dto.Weapon, out weapon))
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                bool isValidPrisoners = true;
                foreach (PrisonerXmlDto dtoPrisoner in dto.Prisoners)
                {
                    if (!context.Prisoners.Any(p => p.Id == dtoPrisoner.Id))
                    {
                        isValidPrisoners = false;
                        break;
                    }
                }

                if (!isValidPrisoners)
                {
                    sb.AppendLine(InvalidData);
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = dto.Name,
                    Salary = dto.Money,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = dto.DepartmentId
                };

                context.Officers.Add(officer);
                context.SaveChanges();
                sb.AppendLine($"Imported {dto.Name} ({dto.Prisoners.Length} prisoners)");

                List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();
                foreach (PrisonerXmlDto dtoPrisoner in dto.Prisoners)
                {
                    OfficerPrisoner officerPrisoner = new OfficerPrisoner()
                    {
                        OfficerId = officer.Id,
                        PrisonerId = dtoPrisoner.Id
                    };

                    officerPrisoners.Add(officerPrisoner);
                }

                context.OfficersPrisoners.AddRange(officerPrisoners);
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
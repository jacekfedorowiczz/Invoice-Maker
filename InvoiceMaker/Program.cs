using AutoMapper;
using InvoiceMaker.Domain.Helpers;
using InvoiceMaker.Domain.Interfaces;
using InvoiceMaker.Domain.Models;

namespace InvoiceMaker
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            ApplicationHelper.InitializeDependencies();

            Console.WriteLine("Program zainicjalizowany. Kliknij dowolny przycisk, aby kontynuowac...");
            Console.ReadKey();

            int select;
            do
            {
                Console.Clear();
                Console.WriteLine("*** Generator faktur v 1.0 ***\n Wybierz funkcje \n 1 - generowanie faktury\n 2 - usuwanie faktury\n 3 - pobranie wszystkich faktur\n 4 - pobranie konkretnej faktury\n 5 - modyfikacja wybranej faktury\n 6 - Pobierz dane z KRS\n 0 - wyjście z aplikacji");

                if (int.TryParse(Console.ReadLine(), out select))
                {
                    Console.Clear();
                    var service = ApplicationHelper.Container.Resolve<IService>();
                    switch (select)
                    {
                        case 1:
                            {
                                service.Create();
                                break;
                            }
                        case 2:
                            {
                                var id = GetId();

                                if (id == 0) break;

                                service.Delete(id);
                                break;
                            }
                        case 3:
                            {
                                var dtos = ApplicationHelper.Container.Resolve<IMapper>().Map<List<InvoiceDto>>(service.GetAll());

                                foreach (var dto in dtos)
                                {
                                    Console.WriteLine($"******* Faktura {dto.Number} *******");
                                    Console.WriteLine(dto.City);
                                    Console.WriteLine(dto.SaleDate);
                                    Console.WriteLine(dto.Total);
                                }

                                break;
                            }
                        case 4:
                            {
                                var id = GetId();

                                if (id == 0) break;

                                var dto = ApplicationHelper.Container.Resolve<IMapper>().Map<InvoiceDto>(service.GetById(id));

                                if (dto is null) break;

                                Console.WriteLine($"******* Faktura {dto.Number}*******");
                                Console.WriteLine(dto.City);
                                Console.WriteLine(dto.SaleDate);
                                Console.WriteLine(dto.Total);

                                break;
                            }
                        case 5:
                            {
                                var id = GetId();

                                if (id == 0) break;

                                service.Update(id);
                                break;
                            }
                        case 6:
                            {
                                Console.Write("Podaj numer KRS: ");
                                var number = Console.ReadLine();
                                Console.Write("Podaj typ rejestru - przedsiębiorców(P) lub stowarzyszeń (S): ");
                                var registry = Console.ReadLine();

                                if (string.IsNullOrEmpty(number) || string.IsNullOrEmpty(registry)) break;

                                var result = service.GetDataFromKRS(number, registry);

                                if (result is null) break;

                                Console.Clear();
                                Console.WriteLine("*************** Dane z KRS ***************");
                                Console.WriteLine($"Nazwa przedsiebiorcy - {result.Name}");
                                Console.WriteLine($"Forma prawna - {result.Form}");
                                Console.WriteLine($"Sposób reprezentacji - {result.Representation}");
                                Console.WriteLine($"Miasto siedziby przedsiebiorcy - {result.City}");
                                Console.WriteLine($"Ulica - {result.Street} {result.StreetNumber}");
                                Console.WriteLine($"Kod pocztowy - {result.PostalCode}\n");
                                break;
                            }
                        default:
                            break;
                    }
                }

                Console.WriteLine("Kliknij dowolny przycisk aby kontynuowac...");
                Console.ReadKey();
            } while (select != 0);
        }

        private static int GetId()
        {
            Console.Clear();
            Console.Write("Podaj numer id faktury: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                return id;
            }

            Console.WriteLine("Nie znaleziono faktury w systemie bądź podano nieprawidłowe ID!");
            return 0;
        }
    }
}

using AutoMapper;
using InvoiceMaker.Builders;
using InvoiceMaker.Domain.Entities;
using InvoiceMaker.Domain.Enums;
using InvoiceMaker.Domain.Helpers;
using InvoiceMaker.Domain.Interfaces;
using InvoiceMaker.Domain.Models;
using Newtonsoft.Json.Linq;

namespace InvoiceMaker.Services
{
    public class InvoiceService : IService
    {
        #region Private fields
        private static IRepository _repository => ApplicationHelper.Container.Resolve<IRepository>();
        private static IMapper _mapper => ApplicationHelper.Container.Resolve<IMapper>();
        #endregion

        #region CRUD
        public void Create()
        {
            Console.Clear();
            Console.WriteLine("*************** Generator faktur ***************");

            Console.Write("Podaj miasto: ");
            var saleCity = Console.ReadLine();

            Console.Write("Podaj date sprzedazy \"d-M-yyyy\": ");
            if(!DateTime.TryParse(Console.ReadLine(), out DateTime saleDate))
            {
                while(DateTime.TryParse(Console.ReadLine(), out saleDate))
                {
                    Console.WriteLine("Podano date w nieprawidlowym formacie! Sprobuj ponownie...");
                }
            }

            var vendor = (Vendor)CreateCredentials(true);
            var vendee = (Vendee)CreateCredentials(false);
            var items = SetItems();
            var paymentMethod = SetPaymentMethod();


            Console.Write("Podaj termin platnosci (\"d-M-yyyy\"): ");
            DateTime.TryParse(Console.ReadLine(), out DateTime paymentDate);
            
            var accountNumber = string.Empty;
            if (paymentMethod == PaymentMethod.Transfer)
            {
                Console.WriteLine("Podaj numer konta bankowego: ");
                accountNumber = Console.ReadLine();
            }


            Invoice invoice = new InvoiceBuilder(_repository)
                                .SetCity(saleCity)
                                .SetSaleDate(saleDate)
                                .SetVendor(vendor)
                                .SetVendee(vendee)
                                .SetItems(items)
                                .SetPaymentMethod(paymentMethod)
                                .SetPaymentDetails(paymentDate, accountNumber)
                                .Build();


            if (invoice is null)
            {
                Console.WriteLine("Nie udało się utworzyć faktury!");
                return;
            }

            _repository.CreateInvoice(invoice);

            Console.WriteLine("Zakończono dodawanie faktury do bazy danych!");
        }

        public void Delete(int id)
        {
            _repository.DeleteInvoice(id);
        }

        public List<InvoiceDto> GetAll()
        {
            var invoices = _repository.GetInvoices();
            return _mapper.Map<List<InvoiceDto>>(invoices);
        }

        public InvoiceDto GetById(int id)
        {
            var invoice = _repository.GetInvoiceById(id);
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public void Update(int id)
        {
            var invoice = _repository.GetInvoiceById(id);

            if (invoice is null) return;

            Console.Clear();
            Console.WriteLine("*************** Modifykator faktur ***************");

            ShowUpdateHelp();

            var dto = _mapper.Map<UpdateInvoiceDto>(invoice);

            while (int.TryParse(Console.ReadLine(), out int select))
            {
                if (select == 9) break;

                switch (select)
                {
                    case 1:
                        {
                            Console.Write("Podaj miasto: ");
                            var saleCity = Console.ReadLine();
                            dto.City = saleCity;
                            break;
                        }
                    case 2:
                        {
                            Console.Write("Podaj date sprzedazy \"d-M-yyyy\": ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime saleDate))
                            {
                                while (DateTime.TryParse(Console.ReadLine(), out saleDate))
                                {
                                    Console.WriteLine("Podano date w nieprawidlowym formacie! Sprobuj ponownie...");
                                }
                            }
                            dto.SaleDate = saleDate;
                            break;
                        }
                    case 3:
                        {
                            Console.Write("Podaj date wystawienia faktury \"d-M-yyyy\": ");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime issueDate))
                            {
                                while (DateTime.TryParse(Console.ReadLine(), out issueDate))
                                {
                                    Console.WriteLine("Podano date w nieprawidlowym formacie! Sprobuj ponownie...");
                                }
                            }
                            dto.IssueDate = issueDate;
                            break;
                        }
                    case 4:
                        {
                            var vendor = (Vendor)CreateCredentials(true);
                            dto.Vendor = vendor;
                            break;
                        }
                    case 5:
                        {
                            var vendee = (Vendee)CreateCredentials(false);
                            dto.Vendee = vendee;
                            break;
                        }
                    case 6:
                        {
                            var items = SetItems();
                            dto.Items = items;
                            break;
                        }
                    case 7:
                        {
                            var paymentMethod = SetPaymentMethod();
                            dto.PaymentMethod = paymentMethod;

                            if (paymentMethod == PaymentMethod.Transfer)
                            {
                                Console.WriteLine("Podaj numer konta bankowego: ");
                                var accountNumber = Console.ReadLine();
                                dto.AccountNumber = accountNumber;
                            }

                            break;
                        }
                    case 8:
                        {
                            Console.Write("Podaj termin platnosci (\"d-M-yyyy\"): ");
                            DateTime.TryParse(Console.ReadLine(), out DateTime paymentDate);
                            dto.PaymentDate = paymentDate;
                            break;
                        }
                    case 0:
                    default:
                        return;
                }

                Console.Clear();
                Console.WriteLine("Zapisano wprowadzone dane!");
                Console.WriteLine("**************************");
                ShowUpdateHelp();
            }

            Console.Clear();
            _repository.UpdateInvoice(dto, invoice);
        }
        #endregion

        #region API request
        public Company GetDataFromKRS(string number, string registry)
        {
            var client = ApplicationHelper.Container.Resolve<IHttpClientFactory>().CreateClient();
            var url = $"https://api-krs.ms.gov.pl/api/krs/OdpisPelny/{number}?rejestr={registry}&format=json";
            var response = Task.Run(() => client.GetAsync(url)).ConfigureAwait(false).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var data = JObject.Parse(responseContent);

                var name = data["odpis"]["dane"]["dzial1"]["danePodmiotu"]["nazwa"][0]["nazwa"].ToString();
                var form = data["odpis"]["dane"]["dzial1"]["danePodmiotu"]["formaPrawna"][0]["formaPrawna"].ToString();
                var representation = data["odpis"]["dane"]["dzial2"]["reprezentacja"][0]["sposobReprezentacji"][0]["sposobReprezentacji"].ToString();
                var city = data["odpis"]["dane"]["dzial1"]["siedzibaIAdres"]["siedziba"][0]["miejscowosc"].ToString();
                var street = data["odpis"]["dane"]["dzial1"]["siedzibaIAdres"]["adres"][0]["ulica"].ToString();
                var postalCode = data["odpis"]["dane"]["dzial1"]["siedzibaIAdres"]["adres"][0]["kodPocztowy"].ToString();
                var streetNumber = data["odpis"]["dane"]["dzial1"]["siedzibaIAdres"]["adres"][0]["nrDomu"].ToString();

                return new Company(name, form, representation, city, street, streetNumber, postalCode);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Private methods
        private void ShowUpdateHelp()
        {
            Console.WriteLine("Jaka pozycje chcesz zaktualizowac?");
            Console.WriteLine("1 - miasto");
            Console.WriteLine("2 - data sprzedazy");
            Console.WriteLine("3 - data wystawienia");
            Console.WriteLine("4 - dane sprzedawcy");
            Console.WriteLine("5 - dane kupujacego");
            Console.WriteLine("6 - towary");
            Console.WriteLine("7 - sposob platnosci");
            Console.WriteLine("8 - termin platnosci");
            Console.WriteLine("9 - zakoncz edytowanie faktury");
            Console.WriteLine("0 - powrot do menu startowego");
        }

        private IPerson CreateCredentials(bool isVendor)
        {
            var contractingParty = isVendor ? "sprzedawcy" : "kupujacego";

            Console.Clear();
            Console.WriteLine($"*************** Generator danych {contractingParty}  ***************");

            Console.Write("Podaj imie: ");
            var firstName = Console.ReadLine();
            Console.Write("Podaj nazwisko: ");
            var lastName = Console.ReadLine();
            Console.Write("Podaj numer telefonu: ");
            var phoneNumber = Console.ReadLine();
            Console.Write("Podaj numer identyfikacji podatkowej: ");
            var taxId = Console.ReadLine();
            Console.Write("Podaj kraj: ");
            var country = Console.ReadLine();
            Console.Write("Podaj miasto: ");
            var city = Console.ReadLine();
            Console.Write("Podaj ulice: ");
            var street = Console.ReadLine();
            Console.Write("Podaj kod pocztowy: ");
            var postalCode = Console.ReadLine();

            if (isVendor)
            {
                return new Vendor(firstName, lastName, phoneNumber, taxId)
                {
                    Country = country,
                    City = city,
                    Street = street,
                    PostalCode = postalCode
                };
            }

            return new Vendee(firstName, lastName, phoneNumber, taxId)
            {
                Country = country,
                City = city,
                Street = street,
                PostalCode = postalCode
            };
        }

        private List<Item> SetItems()
        {
            Console.Clear();
            Console.Write("Podaj ilosc przedmiotow na fakturze: ");

            var items = new List<Item>();

            if(int.TryParse(Console.ReadLine(), out int count))
            {
                for (int i = 0; i < count; i++)
                {
                    Console.Clear();

                    Console.WriteLine($"Podaj nazwe produktu nr {i+1}: ");
                    var name = Console.ReadLine();

                    Console.WriteLine($"Podaj ilosc produktu nr {i + 1}: ");
                    int quantity = 0;
                    int.TryParse(Console.ReadLine(), out quantity);

                    Console.WriteLine($"Podaj cene produktu nr {i + 1}: ");
                    decimal price = 0M;
                    decimal.TryParse(Console.ReadLine(), out price);

                    var item = new Item(name, quantity, price);
                    items.Add(item);
                }

                return items;

            }
            else
            {
                return items;
            }
        }

        private PaymentMethod SetPaymentMethod()
        {
            Console.Clear();
            Console.WriteLine("Podaj rodzaj płatności.");
            Console.WriteLine("1 - płatność przelewem");
            Console.WriteLine("2 - płatność Blikiem");
            Console.WriteLine("3 - płatność przy odbiorze");
            Console.WriteLine("4 - płatność odroczona");
            int.TryParse(Console.ReadLine(), out int value);

            switch (value)
            {
                case 1:
                    return PaymentMethod.Transfer;
                case 2:
                    return PaymentMethod.Blik;
                case 3:
                    return PaymentMethod.OnReceipt;
                case 4:
                    return PaymentMethod.DefferedPayment;
                default:
                    return PaymentMethod.Transfer;
            }

        }
        #endregion
    }
}

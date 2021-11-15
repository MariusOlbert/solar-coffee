using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Customer;
using Xunit;

namespace SolarCoffee.Test
{
    public class TestCustomerService
    {
        [Fact]
        public void CustomerService_GetsAllCustomers_GivenTheyExist() {
            var options = new DbContextOptionsBuilder<SolarDbContext>().UseInMemoryDatabase("gets_all").Options;

            using var context = new SolarDbContext(options);
            var sut = new CustomerService(context);

            sut.CreateCustomer(new Customer{Id = 123123});
            sut.CreateCustomer(new Customer{Id = -222123});

            var allCustomers = sut.GetAllCustomers();
            allCustomers.Count.Should().Be(2);
        }

        [Fact]
        public void CustomerService_CreateCustomer_GivenNewCustomerObject() {
            var options = new DbContextOptionsBuilder<SolarDbContext>().UseInMemoryDatabase("Add_writes_to_database").Options;
            using var context = new SolarDbContext(options);
            var sut = new CustomerService(context);
            sut.CreateCustomer(new Customer{Id = 123123});
            context.Customers.Single().Id.Should().Be(123123);
        }
        [Fact]
        public void CustomerService_DeleteCustomer_GivenId() {
            var options = new DbContextOptionsBuilder<SolarDbContext>().UseInMemoryDatabase("Deletes_one").Options;
            using var context = new SolarDbContext(options);
            var sut = new CustomerService(context);
            sut.CreateCustomer(new Customer{Id = 123123});
            sut.DeleteCustomer(123123);
            var allCustomers = sut.GetAllCustomers();
            allCustomers.Count.Should().Be(0);
        }
        [Fact]
        public void CustomerService_OrdersByLastName_WhenGetAllCustomersInvoked() {
            var data = new List<Customer>
            {
                new Customer {Id = 123, LastName = "Zulu"},
                new Customer {Id = 333, LastName = "Alpha"},
                new Customer {Id = -55, LastName = "Xu"}
            }.AsQueryable();
            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Provider)
                .Returns(data.Provider);
            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Expression)
                .Returns(data.Expression);
            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.ElementType)
                .Returns(data.ElementType);
            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator);

            var mockContext = new Mock<SolarDbContext>();

            mockContext.Setup(c => c.Customers)
                .Returns(mockSet.Object);
            var sut = new CustomerService(mockContext.Object);
            var customers = sut.GetAllCustomers();
            customers.Count.Should().Be(3);
            customers[0].Id.Should().Be(333);
            customers[1].Id.Should().Be(-55);
            customers[2].Id.Should().Be(123);
        }
    }
}

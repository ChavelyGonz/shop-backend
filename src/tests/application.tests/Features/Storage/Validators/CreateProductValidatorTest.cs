using Domain.Enums;
using Application.Features.Storage.DTOs;
using Application.Features.Storage.Commands;
using Application.Features.Storage.Validators;

using FluentValidation.TestHelper;

namespace Application.Tests.Features.Storage.Validators
{
    public class CreateProductValidatorTests
    {
        private readonly CreateProductValidator _validator;

        public CreateProductValidatorTests()
        {
            _validator = new CreateProductValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new CreateProductCommand
            {
                DTO = new ProductDto
                {
                    Name = "",
                    Price = 10.0f,
                    Unit = UnitOfMeasurement.kg
                },
                Stores = new List<int> { 1, 2 }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.DTO.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Price_Is_Negative()
        {
            var command = new CreateProductCommand
            {
                DTO = new ProductDto
                {
                    Name = "Test Product",
                    Price = 0f,
                    Unit = UnitOfMeasurement.kg
                },
                Stores = new List<int> { 1, 2 }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.DTO.Price);
        }

        [Fact]
        public void Should_Have_Error_When_Unit_Is_Invalid()
        {
            var command = new CreateProductCommand
            {
                DTO = new ProductDto
                {
                    Name = "Test Product",
                    Price = 10f,
                    Unit = (UnitOfMeasurement)500
                },
                Stores = new List<int> { 1, 2 }
            };

            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.DTO.Unit);
        }

        [Fact]
        public void Should_Not_Have_Errors_When_Command_Is_Valid()
        {
            var command = new CreateProductCommand
            {
                DTO = new ProductDto
                {
                    Name = "Test Product",
                    Price = 10f,
                    Unit = UnitOfMeasurement.kg
                },
                Stores = new List<int> { 1, 2 }
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

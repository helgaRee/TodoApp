using Azure.Identity;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository)
{
    //Läs ut repo
    private readonly UserRepository _userRepository = userRepository;

    //skapa metod för att skapa en kontakt, skicka med Dto
    //om kontakten inte finns, skapa kontakten. Om Kontakten finns, hämta kontakten och läs ut.

    public async Task<UserEntity> CreateUserAsync(UserRegistrationDto userRegistrationDto)
    {
        try
        {
            if (!await _userRepository.ExistingAsync(x => x.Email == userRegistrationDto.Email))
            {
                var newUser = new UserEntity
                {
                    UserName = userRegistrationDto.UserName,
                    Email = userRegistrationDto.Email,
                    Password = userRegistrationDto.Password,
                };
                newUser = await _userRepository.CreateAsync(newUser);
                return newUser;
            }
            else
            {
                var existingUser = await _userRepository.GetAsync(x => x.Email == userRegistrationDto.Email);
                return existingUser;
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }

    //skapa en metod för att ta bort en User
    //1. gör en sökning, Finns den här användarens email?
    //2. finns den , ta bort och returnera true.
    //3. finns den ej, return false.
    public async Task<bool> DeleteUserAsync(Expression<Func<UserEntity, bool>> expression)
    {
        try
        {
            var result = await _userRepository.DeleteAsync(expression);
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return false;
    }
}

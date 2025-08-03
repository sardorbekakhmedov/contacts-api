using AutoMapper;
using ContactsApi.Dtos;
using ContactsApi.Entities;
using ContactsApi.Models;

namespace ContactsApi.Services.Contacts;

public class ContactMapping : Profile
{
    public ContactMapping()
    {
        CreateMap<CreateContactDto, CreateContact>();
        CreateMap<UpdateContactDto, UpdateContact>();
        CreateMap<PatchContactDto, PatchContact>();

        CreateMap<CreateContact, Contact>();
        CreateMap<UpdateContact, Contact>();
        CreateMap<Contact, ViewContact>();
    }
}
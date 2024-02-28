﻿using AutoMapper;
using Business.Abstracts;
using Business.Requests.Employees;
using Business.Responses.Applicants;
using Business.Responses.BootcampStates;
using Business.Responses.Employees;
using Core.DataAccess;
using Core.Exceptions.Types;
using Core.Utilities.Results;
using DataAccess.Abstracts;
using DataAccess.Repositories;
using Entities.Concrates;
using Entities.Concretes;
using Microsoft.EntityFrameworkCore;

namespace Business.Concretes;

public class EmployeeManager : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public EmployeeManager(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _repository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<IDataResult<CreateEmployeeResponse>> AddAsync(CreateEmployeeRequest request)
    {
        await CheckIfEmployeeNotExists(request.UserName, request.NationalIdentity);
        Employee employee = _mapper.Map<Employee>(request);
        await _repository.AddAsync(employee);

        CreateEmployeeResponse response = _mapper.Map<CreateEmployeeResponse>(employee);
        return new SuccessDataResult<CreateEmployeeResponse>(response, "Ekleme Başarılı");

    }

    public async Task<IResult> DeleteAsync(DeleteEmployeeRequest request)
    {
        await CheckIfIdNotExists(request.Id);
        Employee employee = await _repository.GetAsync(x => x.Id == request.Id);
        await _repository.DeleteAsync(employee);
        return new SuccessResult("Silme Başarılı");
    }

    public async Task<IDataResult<List<GetAllEmployeeResponse>>> GetAll()
    {
        List<Employee> employee = await _repository.GetAllAsync();
        List<GetAllEmployeeResponse> responses = _mapper.Map<List<GetAllEmployeeResponse>>(employee);
        return new SuccessDataResult<List<GetAllEmployeeResponse>>(responses, "Listeleme Başarılı");
    }

    public async Task<IDataResult<GetByIdEmployeeResponse>> GetById(int id)
    {
        await CheckIfIdNotExists(id);
        Employee employee= await _repository.GetAsync(x => x.Id == id);

        GetByIdEmployeeResponse response = _mapper.Map<GetByIdEmployeeResponse>(employee);
        return new SuccessDataResult<GetByIdEmployeeResponse>(response, "GetById İşlemi Başarılı");

    }

    public async Task<IDataResult<UpdateEmployeeResponse>> UpdateAsync(UpdateEmployeeRequest request)
    {
        await CheckIfIdNotExists(request.Id);
        Employee employee = await _repository.GetAsync(x => x.Id == request.Id);
        _mapper.Map(request,employee);
        await _repository.UpdateAsync(employee);

        UpdateEmployeeResponse response = _mapper.Map<UpdateEmployeeResponse>(employee);
        return new SuccessDataResult<UpdateEmployeeResponse>(response, "Update İşlemi Başarılı");
    }
    private async Task CheckIfIdNotExists(int employeeId)
    {
        var isExists = await _repository.GetAsync(x => x.Id == employeeId);
        if (isExists is null) throw new BusinessException("Id not exists");
    }

    private async Task CheckIfEmployeeNotExists(string userName, string nationalIdentity)
    {
        var isExists = await _repository.GetAsync(x => x.UserName == userName || x.NationalIdentity == nationalIdentity);
        if (isExists is not null) throw new BusinessException("UserName or National Identity is already exists");
    }
}

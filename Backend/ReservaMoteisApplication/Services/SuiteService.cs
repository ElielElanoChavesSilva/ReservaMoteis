using BookMotelsApplication.DTOs.Suite;
using BookMotelsApplication.Interfaces;
using BookMotelsApplication.Mappers;
using BookMotelsDomain.Entities;
using BookMotelsDomain.Exceptions;
using BookMotelsDomain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text.Json;

namespace BookMotelsApplication.Services;

public class SuiteService : ISuiteService
{
    private readonly ISuiteRepository _suiteRepository;
    private readonly IMotelRepository _motelRepository;
    private readonly IDistributedCache _cache;

    public SuiteService(
        IMotelRepository motelRepository,
        ISuiteRepository suiteRepository,
        IDistributedCache cache)
    {
        _cache = cache;
        _motelRepository = motelRepository;
        _suiteRepository = suiteRepository;
    }

    public async Task<IEnumerable<GetSuiteDTO>> FindAllAsync()
    {
        IEnumerable<SuiteEntity> suites = await _suiteRepository.FindAll();
        return suites.ToDTO();
    }

    public async Task<IEnumerable<GetSuiteDTO>> FindAllAvailable(long motelId, string? name, DateTime? checkin, DateTime? checkout)
    {
        if (!await _motelRepository.Exist(motelId))
            throw new NotFoundException("Suíte não encontrada");

        var cacheKey = $"suites:available:{motelId}:{name}:{checkin:yyyyMMddHH}:{checkout:yyyyMMddHH}";

        var cached = await _cache.GetStringAsync(cacheKey);
        if (cached != null)
            return JsonConvert.DeserializeObject<IEnumerable<GetSuiteDTO>>(cached)!;

        IEnumerable<SuiteEntity> suites = (await _suiteRepository.FindAllAvailable(motelId, name, checkin, checkout)).ToList();

        var json = System.Text.Json.JsonSerializer.Serialize(
            suites.ToDTO(),
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );

        await _cache.SetStringAsync(cacheKey, json,
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) }
        );

        return suites.ToDTO();
    }

    public async Task<GetSuiteDTO> FindByIdAsync(long id)
    {
        SuiteEntity suite = await _suiteRepository.FindById(id) ??
                            throw new NotFoundException("Suíte não encontrada");

        return suite.ToDTO();
    }

    public async Task<GetSuiteDTO> AddAsync(long motelId, SuiteDTO suiteDto)
    {
        if (!await _motelRepository.Exist(motelId))
            throw new NotFoundException("Motel não encontrado");

        byte[]? imageBytes = null;

        if (suiteDto.Image is not null)
        {
            using var ms = new MemoryStream();
            await suiteDto.Image.CopyToAsync(ms);
            imageBytes = ms.ToArray();
        }

        SuiteEntity entity = suiteDto.ToEntity();
        entity.MotelId = motelId;
        entity.ImageUrl = imageBytes;
        entity = await _suiteRepository.Add(entity);

        string generalCacheKey = $"suites:available:{motelId}:::";
        await _cache.RemoveAsync(generalCacheKey);

        return entity.ToDTO();
    }

    public async Task UpdateAsync(long id, SuiteDTO suiteDto)
    {
        SuiteEntity existingSuite = await _suiteRepository.FindById(id) ??
                                    throw new NotFoundException("Suíte não encontrada");

        existingSuite.Name = suiteDto.Name;
        existingSuite.Description = suiteDto.Description;
        existingSuite.PricePerPeriod = suiteDto.PricePerPeriod;
        existingSuite.MaxOccupancy = suiteDto.MaxOccupancy;

        await _suiteRepository.Update(existingSuite);

        string generalCacheKey = $"suites:available:{existingSuite.MotelId}:::";
        await _cache.RemoveAsync(generalCacheKey);
    }

    public async Task DeleteAsync(long id)
    {
        SuiteEntity entity = await _suiteRepository.FindById(id) ??
                             throw new NotFoundException("Suíte não encontrada");

        await _suiteRepository.Delete(entity);

        string generalCacheKey = $"suites:available:{entity.MotelId}:::";
        await _cache.RemoveAsync(generalCacheKey);
    }
}
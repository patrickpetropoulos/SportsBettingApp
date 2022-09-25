using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SB.Server.Root.Casinos;

namespace SB.Server.WebApp.IntegrationTests;

public class CasinosTests
{
    public static Casino CreateTestCasino()
    {
        return new Casino()
        {
            Id = Guid.NewGuid(),
            CountryCode = "CA",
            Name = "DELETE ME, IM TEST DATA"
        };
    }
    public static async Task<List<Casino>> GetAllCasinos(HttpClient client)
    {
        var response = await client.GetAsync( "/api/casinos/" );
        
        Assert.AreEqual( HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse( result );
        
        return JsonConvert.DeserializeObject<List<Casino>>(((JArray?) json?["casinos"]).ToString());
    }

    public static async Task<Casino> CreateCasino(HttpClient client, Casino casino)
    {
        var response = await client.PostAsJsonAsync("/api/casinos/", 
            new{
                Id = casino.Id,
                CountryCode = casino.CountryCode,
                Name = casino.Name});
        
        Assert.AreEqual( HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse( result );
        
        return JsonConvert.DeserializeObject<Casino>(((JObject?) json?["casino"]).ToString());
    }
    
    public static async Task<Casino> UpdateCasino(HttpClient client, Casino casino)
    {
        var response = await client.PutAsJsonAsync($"/api/casinos/{casino.Id}", 
            new{
                Id = casino.Id,
                CountryCode = casino.CountryCode,
                Name = casino.Name});
        
        Assert.AreEqual( HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse( result );
        
        return JsonConvert.DeserializeObject<Casino>(((JObject?) json?["casino"]).ToString());
    }
    
    public static async Task DeleteCasino(HttpClient client, Guid id)
    {
        var deleteResponse = await client.DeleteAsync($"/api/casinos/{id}");
        
        Assert.AreEqual( HttpStatusCode.OK, deleteResponse.StatusCode);
    }

    public static void ValidateCasinoNotInList(List<Casino> casinos, Casino casino)
    {
        if (casinos.Any(c => c.BasicDataEquals(casino)))
        {
            Assert.Fail();
        }
    }
    
    [Test]
    public async Task GetAllCasinos_EnsureContainsFullSeedDataList()
    {
        var client = await Config.GetAuthorizedClientWithoutAdminAccessLevel();

        var casinoList = await GetAllCasinos(client);

        var expectedList = DbSeeding.GetSeedCasinos();

        //If any of the known seed data casinos do not appear in list, Fail
        foreach (var casino in expectedList)
        {
            if (!casinoList.Any(c => c.BasicDataEquals(casino)))
            {
                Assert.Fail();
            }
        }
    }

    [Test]
    public async Task CreateCasino_ThenDelete_EnsureNotInNewListOfAllCasinos()
    {
        var client = await Config.GetAuthorizedClientWithAdminAccessLevel();
        var casinoToCreate = CreateTestCasino();

        var createdCasino = await CreateCasino(client, casinoToCreate);

        //Make sure created casino is correct
        Assert.IsTrue(createdCasino.BasicDataEquals(casinoToCreate));

        await DeleteCasino(client, createdCasino.Id);

        //Get all casinos and ensure new casino is not in the list
        var casinoList = await GetAllCasinos(client);
        ValidateCasinoNotInList(casinoList, casinoToCreate);
    }

    [Test]
    public async Task CreateCasino_ThenUpdateAndValidateChanges_ThenDelete_EnsureNotInNewListofAllCasinos()
    {
        var client = await Config.GetAuthorizedClientWithAdminAccessLevel();
        var casinoToCreate = CreateTestCasino();

        var createdCasino = await CreateCasino(client, casinoToCreate);
        //Make sure created casino is correct
        Assert.IsTrue(createdCasino.BasicDataEquals(casinoToCreate));

        createdCasino.Name = "THIS IS MY NEW NAME";
        createdCasino.CountryCode = "US";

        var updatedCasino = await UpdateCasino(client, createdCasino);
        
        Assert.IsTrue(createdCasino.BasicDataEquals(updatedCasino));
        
        await DeleteCasino(client, updatedCasino.Id);

        //Get all casinos and ensure new casino is not in the list
        var casinoList = await GetAllCasinos(client);
        ValidateCasinoNotInList(casinoList, updatedCasino);

    }
}
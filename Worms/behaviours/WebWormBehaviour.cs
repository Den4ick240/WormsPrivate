using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Worms.abstractions;
using Worms.Domain;
using Worms.entities;
using Worms.entities.apiExtensions;
using Worm = Worms.entities.Worm;

namespace Worms
{
    static class MDF
    {
        public static T RunSynchronouslyAndUnwrapException<T>(this Task<T> task)
        {
            task.Wait();

            if (task.Exception != null) throw task.Exception.GetBaseException();

            return task.Result;
        }
    }
    public class WebWormBehaviour : AbstractWormBehaviour
    {
        private readonly HttpClient _client;

        public WebWormBehaviour(ActionFactory actionFactory, HttpClient httpClient) : base(actionFactory)
        {
            _client = httpClient;
        }

        public override IWormAction GetAction(World world, Worm worm)
        {
            var httpResponseMessage = _client.PostAsJsonAsync(
                $"http://localhost:5000/worm/{worm.Name}/getAction?",
                world.ToApi())
                .RunSynchronouslyAndUnwrapException();
            
            httpResponseMessage.EnsureSuccessStatusCode();
            
            var response = httpResponseMessage
                .Content
                .ReadFromJsonAsync<Response>()
                .RunSynchronouslyAndUnwrapException();
           
            return _actionFactory.GetActionFromApi(response);
        }
    }
}
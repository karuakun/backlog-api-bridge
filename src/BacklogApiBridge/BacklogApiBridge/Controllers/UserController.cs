using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BacklogApiBridge.Backlog;
using BacklogApiBridge.Backlog.Models;
using BacklogApiBridge.Extensions;
using BacklogApiBridge.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BacklogApiBridge.Controllers
{
    /// <summary>
    /// BacklogユーザーのAPI呼び出しを行います。
    /// </summary>
    [AllowSpace]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// ユーザーの一覧を検索します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <returns></returns>
        public async Task<User[]> GetUsers(
            [Required] string apiKey,
            [Required] string spaceKey
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetUsersAsync(apiKey);

        /// <summary>
        /// メールアドレスからユーザーを検索します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="mailAddress"></param>
        /// <returns></returns>
        [Route("findByMailAddress")]
        public async Task<User> FindUsersByMailAddress(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string mailAddress)
        {
            var client = _httpClientFactory.CreateRestServiceClient<IBacklogClient>(spaceKey);
            var users = await client.GetUsersAsync(apiKey);
            return users.FirstOrDefault(u => u.MailAddress == mailAddress);
        }

        /// <summary>
        /// ユーザーの情報を取得します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns></returns>
        [Route("{userId}")]
        public async Task<User> GetUser(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] int userId
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetUserAsync(apiKey, userId);
    }
}
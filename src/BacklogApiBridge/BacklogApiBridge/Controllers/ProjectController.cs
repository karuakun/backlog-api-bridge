using System.ComponentModel.DataAnnotations;
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
    /// Backlogプロジェクトに対するAPIの呼び出しを行います。
    /// </summary>
    [AllowSpace]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public ProjectController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// プロジェクトの一覧を取得します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <returns></returns>
        public async Task<ActionResult<Project[]>> GetProjects(
            [Required] string apiKey,
            [Required] string spaceKey
        ) => 
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetProjectsAsync(apiKey);

        /// <summary>
        /// プロジェクトの情報を取得します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}")]
        public async Task<ActionResult<Project>> GetProject(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey
        ) => 
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetProjectAsync(apiKey, projectIdOrKey);

        /// <summary>
        /// プロジェクトのアイコン画像を取得します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/image")]
        public async Task<IActionResult> GetImage(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey
        )
        {
            var client = _httpClientFactory.CreateRestServiceClient<IBacklogClient>(spaceKey);
            var data = await client.GetProjectImageAsync(apiKey, projectIdOrKey);
            return File(await data.ReadAsByteArrayAsync(), 
                data.Headers.ContentType.MediaType,
                string.IsNullOrEmpty(data.Headers.ContentDisposition.FileNameStar) 
                    ? string.Empty
                    : data.Headers.ContentDisposition.FileNameStar);
        }

        /// <summary>
        /// プロジェクト上の最近の活動の一覧を取得します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="activityTypeIds"></param>
        /// <param name="minId">最小の課題ID</param>
        /// <param name="maxId">最大の課題ID</param>
        /// <param name="count">取得上限(1-100) 指定が無い場合は100</param>
        /// <param name="order">“asc”または”desc” 指定が無い場合は”desc”</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/activities")]
        public async Task<ActionResult<Activity[]>> GetActivities(
            [Required] string apiKey,
            [Required] string spaceKey,
            string activityTypeIds = "[]",
            int? minId = null,
            int? maxId = null,
            int? count = 100,
            string order = "desc"
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetSpaceActivitiesAsync(
                    apiKey, 
                    activityTypeIds.FromJsonToIntArray(), 
                    minId, 
                    maxId, 
                    count, 
                    order);

        /// <summary>
        /// プロジェクト管理者に設定されているユーザーの一覧を取得します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/administrators")]
        public async Task<ActionResult<User[]>> GetAdministrators(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey
        ) =>
            await _httpClientFactory
                    .CreateRestServiceClient<IBacklogClient>(spaceKey)
                    .GetProjectAdministratorsAsync(apiKey, projectIdOrKey);

        /// <summary>
        /// プロジェクト固有の課題に設定できる状態一覧を取得します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/statuses")]
        public async Task<ActionResult<Status[]>> GetStatuses(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetProjectStatusesAsync(apiKey, projectIdOrKey);

        /// <summary>
        /// プロジェクトに登録されている種別の一覧を返します
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/issueTypes")]
        public async Task<ActionResult<IssueType[]>> GetIssueTypes(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetProjectIssueTypesAsync(apiKey, projectIdOrKey);

        /// <summary>
        /// プロジェクトに登録されているカテゴリーの一覧を返します
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/categories")]
        public async Task<ActionResult<Category[]>> GetCategories(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetProjectCategoriesAsync(apiKey, projectIdOrKey);

        /// <summary>
        /// プロジェクトに登録されているカスタムフィールドの一覧を返します
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="spaceKey"></param>
        /// <param name="projectIdOrKey"></param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/customFields")]
        public async Task<ActionResult<CustomField[]>> GetCustomFields(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetProjectCustomFieldsAsync(apiKey, projectIdOrKey);

        /// <summary>
        /// プロジェクトユーザー一覧の取得
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <param name="excludeGroupMembers">グループを介してプロジェクトに参加しているメンバーを除く</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/users")]
        public async Task<ActionResult<User[]>> GetUsers(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey,
            bool excludeGroupMembers = false
        ) => 
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetProjectUsersAsync(apiKey, projectIdOrKey, excludeGroupMembers);

        /// <summary>
        /// バージョン（マイルストーン）一覧の取得
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/versions")]
        public async Task<ActionResult<BacklogVersion[]>> GetVersions(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetProjectVersionsAsync(apiKey, projectIdOrKey);

        /// <summary>
        /// プロジェクトに紐づくWebhookの一覧を取得します。この操作はBacklogもしくはProjectの管理者が実施する必要があります。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/webhooks")]
        public async Task<ActionResult<Webhook[]>> GetWebhooks(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetWebhooksAsync(apiKey, projectIdOrKey);

        /// <summary>
        /// プロジェクトに紐づくWebhookを取得します。この操作はBacklogもしくはProjectの管理者が実施する必要があります。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <param name="webhookId">処理対象のWebhookのId</param>
        /// <returns></returns>
        [Route("{projectIdOrKey}/webhooks/{webhookId}")]
        public async Task<ActionResult<Webhook>> GetWebhook(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey,
            [Required] string webhookId
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetWebhookAsync(apiKey, projectIdOrKey, webhookId);
        /// <summary>
        /// プロジェクトに紐づくWebhookを作成します。この操作はBacklogもしくはProjectの管理者が実施する必要があります。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{projectIdOrKey}/webhooks/{webhookId}")]
        public async Task<ActionResult<Webhook>> CreateWebhook(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .CreateWebhookAsync(apiKey, projectIdOrKey);
        /// <summary>
        /// プロジェクトに紐づくWebhookを更新します。この操作はBacklogもしくはProjectの管理者が実施する必要があります。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <param name="webhookId">処理対象のWebhookのId</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{projectIdOrKey}/webhooks/{webhookId}")]
        public async Task<ActionResult<Webhook>> UpdateWebhook(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey,
            [Required] string webhookId
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .UpdateWebhookAsync(apiKey, projectIdOrKey, webhookId);
        /// <summary>
        /// プロジェクトに紐づくWebhookを削除します。この操作はBacklogもしくはProjectの管理者が実施する必要があります。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <param name="webhookId">処理対象のWebhookのId</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{projectIdOrKey}/webhooks/{webhookId}")]
        public async Task<ActionResult<Webhook>> DeleteWebhook(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey,
            [Required] string webhookId
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .DeleteWebhookAsync(apiKey, projectIdOrKey, webhookId);

        /// <summary>
        /// プロジェクトに紐づくGITリポジトリのPullRequestを作成します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <param name="repoIdOrName"></param>
        /// <param name="summary"></param>
        /// <param name="description"></param>
        /// <param name="base"></param>
        /// <param name="branch"></param>
        /// <param name="issueId"></param>
        /// <param name="assigneeId"></param>
        /// <param name="notifiedUserIds"></param>
        /// <param name="attachmentIds"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{projectIdOrKey}/git/repositories/{repoIdOrName}/pullRequests")]
        public async Task<ActionResult<PullRequest>> CreatePullRequest(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey,
            [Required] string repoIdOrName,
            [Required] string summary,
            [Required] string description,
            [Required] string @base,
            [Required] string branch,
            int? issueId,
            int? assigneeId,
            string notifiedUserIds = "[]",
            string attachmentIds = "[]"
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .CreatePullRequestAsync(
                    apiKey,
                    projectIdOrKey,
                    repoIdOrName,
                    summary,
                    description,
                    @base,
                    branch,
                    issueId,
                    assigneeId,
                    notifiedUserIds.FromJsonToIntArray(),
                    attachmentIds.FromJsonToIntArray());

        /// <summary>
        /// プロジェクトに紐づくGITリポジトリのPullRequestを取得します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <param name="repoIdOrName"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{projectIdOrKey}/git/repositories/{repoIdOrName}/pullRequests/{number}")]
        public async Task<ActionResult<PullRequest>> GetPullRequest(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey,
            [Required] string repoIdOrName,
            [Required] int number
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .GetPullRequestAsync(
                    apiKey,
                    projectIdOrKey,
                    repoIdOrName,
                    number);

        /// <summary>
        /// プロジェクトに紐づくGITリポジトリのPullRequestを削除します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIdOrKey">プロジェクトのID または プロジェクトキー</param>
        /// <param name="repoIdOrName"></param>
        /// <param name="number"></param>
        /// <param name="summary"></param>
        /// <param name="description"></param>
        /// <param name="issueId"></param>
        /// <param name="assigneeId"></param>
        /// <param name="notifiedUserIds"></param>
        /// <param name="comments"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{projectIdOrKey}/git/repositories/{repoIdOrName}/pullRequests/{number}")]
        public async Task<ActionResult<PullRequest>> DeletePullRequest(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string projectIdOrKey,
            [Required] string repoIdOrName,
            [Required] int number,
            [Required] string summary,
            [Required] string description,
            int? issueId,
            int? assigneeId,
            string notifiedUserIds = "[]",
            string comments = null
        ) =>
            await _httpClientFactory
                .CreateRestServiceClient<IBacklogClient>(spaceKey)
                .UpdatePullRequestAsync(
                    apiKey,
                    projectIdOrKey,
                    repoIdOrName,
                    number,
                    summary,
                    description,
                    issueId,
                    assigneeId,
                    notifiedUserIds.FromJsonToIntArray(),
                    comments);
    }
}
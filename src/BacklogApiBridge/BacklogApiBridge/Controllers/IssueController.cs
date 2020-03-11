using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BacklogApiBridge.Backlog;
using BacklogApiBridge.Backlog.Models;
using BacklogApiBridge.CustomModel;
using BacklogApiBridge.Extensions;
using BacklogApiBridge.Filters;
using BacklogApiBridge.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BacklogApiBridge.Controllers
{
    /// <summary>
    /// Backlog 課題のAPI呼び出しを行います。
    /// </summary>
    [AllowSpace]
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public IssueController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private const int MaxPageSize = 100;

        /// <summary>
        /// 様々な条件でBacklogの課題を検索します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="projectIds">プロジェクトのID</param>
        /// <param name="issueTypeIds">種別のID</param>
        /// <param name="categoryIds">カテゴリーのID</param>
        /// <param name="versionIds">発生バージョンのID</param>
        /// <param name="milestoneIds">マイルストーンのID</param>
        /// <param name="statusIds">状態のID</param>
        /// <param name="priorityIds">優先度のID</param>
        /// <param name="assigneeIds">担当者のID</param>
        /// <param name="createdUserIds">登録者のID</param>
        /// <param name="resolutionIds">完了理由のID</param>
        /// <param name="parentChild">親子課題の条件 0: すべて, 1: 子課題以外, 2: 子課題, 3: 親課題でも子課題でもない課題, 4: 親課題</param>
        /// <param name="attachment">添付ファイルを含む場合はtrue</param>
        /// <param name="sharedFile">共有ファイルを含む場合はtrue</param>
        /// <param name="sort">課題一覧のソートに使用する属性名</param>
        /// <param name="order">“asc”または”desc” 指定が無い場合は”desc”</param>
        /// <param name="offset"></param>
        /// <param name="count">取得上限(1-100) 指定が無い場合は100</param>
        /// <param name="createdSince">登録日(yyyy-mm-dd)</param>
        /// <param name="createdUntil">登録日(yyyy-mm-dd)</param>
        /// <param name="updatedSince">更新日(yyyy-mm-dd)</param>
        /// <param name="updatedUntil">更新日(yyyy-mm-dd)</param>
        /// <param name="startDateSince">開始日(yyyy-mm-dd)</param>
        /// <param name="startDateUntil">開始日(yyyy-mm-dd)</param>
        /// <param name="dueDateSince">期限日(yyyy-mm-dd)</param>
        /// <param name="dueDateUntil">期限日(yyyy-mm-dd)</param>
        /// <param name="ids">課題のID</param>
        /// <param name="parentIssueIds">親課題のID</param>
        /// <param name="keyWord">検索キーワード</param>
        /// <returns></returns>
        public async Task<Issue[]> FindByAsync(
            [Required] string apiKey,
            [Required] string spaceKey,
            string projectIds = "[]",
            string issueTypeIds = "[]",
            string categoryIds = "[]",
            string versionIds = "[]",
            string milestoneIds = "[]",
            string statusIds = "[]",
            string priorityIds = "[]",
            string assigneeIds = "[]",
            string createdUserIds = "[]",
            string resolutionIds = "[]",
            ParentChild parentChild = ParentChild.All,
            bool attachment = false,
            bool sharedFile = false,
            string sort = "created",
            string order = "desc",
            int offset = 0,
            int count = 100,
            DateTime? createdSince = null,
            DateTime? createdUntil = null,
            DateTime? updatedSince = null,
            DateTime? updatedUntil = null,
            DateTime? startDateSince = null,
            DateTime? startDateUntil = null,
            DateTime? dueDateSince = null,
            DateTime? dueDateUntil = null,
            string ids = "[]",
            string parentIssueIds = "[]",
            string keyWord = null)
        {
            if (offset < 0) return null;
            if (count <= 0) return null;
            var pageSize = AdjustPageSize(count);

            var results = new List<Issue>();
            for (var i = 0; i < (int)Math.Ceiling((double)count / MaxPageSize); i++)
            {
                var currentOffset = offset + i * pageSize;
                var remainingSize = count - currentOffset;
                var currentCount = AdjustPageSize(remainingSize);

                var issues = await _httpClientFactory
                    .CreateRestServiceClient<IBacklogClient>(spaceKey)
                    .FindIssiesByAsync(apiKey,
                        projectIds.FromJsonToIntArray(),
                        issueTypeIds.FromJsonToIntArray(),
                        categoryIds.FromJsonToIntArray(),
                        versionIds.FromJsonToIntArray(),
                        milestoneIds.FromJsonToIntArray(),
                        statusIds.FromJsonToIntArray(),
                        priorityIds.FromJsonToIntArray(),
                        assigneeIds.FromJsonToIntArray(),
                        createdUserIds.FromJsonToIntArray(),
                        resolutionIds.FromJsonToIntArray(),
                        (int)parentChild,
                        attachment.ToString().ToLower(),
                        sharedFile.ToString().ToLower(),
                        sort,
                        order,
                        currentOffset,
                        currentCount,
                        createdSince?.ToString("yyyy-MM-dd"),
                        createdUntil?.ToString("yyyy-MM-dd"),
                        updatedSince?.ToString("yyyy-MM-dd"),
                        updatedUntil?.ToString("yyyy-MM-dd"),
                        startDateSince?.ToString("yyyy-MM-dd"),
                        startDateUntil?.ToString("yyyy-MM-dd"),
                        dueDateSince?.ToString("yyyy-MM-dd"),
                        dueDateUntil?.ToString("yyyy-MM-dd"),
                        ids.FromJsonToIntArray(),
                        parentIssueIds.FromJsonToIntArray(),
                        keyWord);
                if (issues == null || !issues.Any())
                {
                    break;
                }
                results.AddRange(issues);
            }
            return results.ToArray();
        }

        /// <summary>
        /// この課題のステータスの最終更新状態を取得します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="issueIdOrKey">課題のID または 課題キー</param>
        /// <returns></returns>
        [Route("{issueIdOrKey}/lastStatusChange")]
        public async Task<StatusChange> GetLastStatusChangeLog(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string issueIdOrKey)
        {
            var comments = await GetComments(apiKey, spaceKey, issueIdOrKey);
            return comments
                .Select(c =>
                {
                    var status = c.ChangeLog.FirstOrDefault(l => l.Field == "status") ?? new ChangeLog();
                    return new StatusChange
                    {
                        IssueId = c.Id,
                        UpdateUser = c.CreatedUser,
                        Updated = c.Created,
                        NewValue = status?.NewValue,
                        OriginalValue = status?.OriginalValue,
                        Comment = c
                    };
                }).FirstOrDefault();
        }
        /// <summary>
        /// 課題のコメントを取得します。
        /// </summary>
        /// <param name="apiKey">BacklogAPIを呼び出すためのAPIKey</param>
        /// <param name="spaceKey">組織キー https://xxxx.backlog.jp/ のxxxの部分</param>
        /// <param name="issueIdOrKey">課題のID または 課題キー</param>
        /// <param name="minId">最小ID</param>
        /// <param name="maxId">最大ID</param>
        /// <param name="count">取得上限(1-100) 指定が無い場合は100</param>
        /// <param name="order">"asc"または"desc"指定が無い場合は"desc"</param>
        /// <returns></returns>
        [Route("{issueIdOrKey}/comments")]
        public async Task<Comment[]> GetComments(
            [Required] string apiKey,
            [Required] string spaceKey,
            [Required] string issueIdOrKey,
            int? minId = null,
            int? maxId = null,
            int count = 100,
            string order = "desc")
        {
            if (count <= 0) return null;

            var results = new List<Comment>();
            for (var i = 0; i < (int) Math.Ceiling((double) count / MaxPageSize); i++)
            {
                var comments = await _httpClientFactory
                    .CreateRestServiceClient<IBacklogClient>(spaceKey)
                    .GetCommentsAsync(
                        apiKey,
                        issueIdOrKey,
                        minId,
                        maxId,
                        AdjustPageSize(count),
                        order);
                if (comments == null || !comments.Any())
                {
                    break;
                }
                // 2回目以降のリクエストはminidとmaxidを置き換える
                if (order == "desc")
                {
                    maxId = comments.Max(c => c.Id);
                }
                else
                {
                    minId = comments.Min(c => c.Id);
                }
                results.AddRange(comments);
            }
            return results.ToArray();
        }
        
        private int AdjustPageSize(int pageSize) =>
            MaxPageSize <= pageSize ? MaxPageSize : pageSize;
    }
}
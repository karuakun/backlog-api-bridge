using System;
using System.Collections.Generic;
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
    /// Backlog スペースのAPI呼び出しを行います。
    /// </summary>
    [AllowSpace]
    [Route("api/[controller]")]
    [ApiController]
    public class SpaceController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const int MaxPageSize = 100;

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public SpaceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// スペース情報を取得します
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="spaceKey"></param>
        /// <returns></returns>
        public async Task<ActionResult<Space>> Space(string apiKey, string spaceKey) =>
            await _httpClientFactory.CreateRestServiceClient<IBacklogClient>(spaceKey).GetSpaceAsync(apiKey);

        /// <summary>
        /// 最近の更新の取得
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="spaceKey"></param>
        /// <param name="activityTypeIds"></param>
        /// <param name="minId"></param>
        /// <param name="maxId"></param>
        /// <param name="count">取得上限(1-100) 指定が無い場合は20</param>
        /// <param name="order">ascまたはdesc指定が無い場合はdesc</param>
        /// <returns></returns>
        [Route("Activities")]
        public async Task<ActionResult<Activity[]>> GetActivities(
            [Required] string apiKey,
            [Required] string spaceKey,
            string activityTypeIds,
            int? minId = null,
            int? maxId = null,
            int count = 100,
            string order = "desc")
        {
            if (count <= 0) return null;

            var result = new List<Activity>();
            for (var i = 0; i < (int) Math.Ceiling((double) count / MaxPageSize); i++)
            {
                var activities = await _httpClientFactory.
                    CreateRestServiceClient<IBacklogClient>(spaceKey).
                    GetSpaceActivitiesAsync(
                        apiKey,
                        activityTypeIds.FromJsonToIntArray(),
                        minId,
                        maxId,
                        AdjustPageSize(count),
                        order);
                if (activities == null || !activities.Any())
                {
                    break;
                }
                // 2回目以降のリクエストはminidとmaxidを置き換える
                if (order == "desc")
                {
                    maxId = activities.Max(c => c.Id);
                }
                else
                {
                    minId = activities.Min(c => c.Id);
                }
                result.AddRange(activities);
            }
            return result.ToArray();
        }
        private int AdjustPageSize(int pageSize) =>
            MaxPageSize <= pageSize ? MaxPageSize : pageSize;

    }
}
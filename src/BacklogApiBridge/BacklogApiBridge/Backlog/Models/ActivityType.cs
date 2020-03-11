namespace BacklogApiBridge.Backlog.Models
{
    public enum ActivityType
    {
        AddIssue = 1,
        UpdateIssue = 2,
        CommentForIssue = 3,
        DeleteIssue = 4,
        AddWiki = 5,
        UpdateWiki = 6,
        DeleteWiki = 7,
        AddShardFile = 8,
        UpdateSharedFile = 9,
        DeleteSharedFile = 10,
        CommitSubversion = 11,
        PushGit = 12,
        CreateGitRepository = 13,
        UpdateMultiIssue = 14,
        AssignUser = 15,
        ReleaseUser = 16,
        AddNotificationForComment = 17,
        AddPullRequest = 18,
        UpdatePullRequest = 19,
        AddCommentsForPullRequest = 20,
        DeletePullRequest = 21,
        AddMilestone = 22,
        UpdateMilestone = 23,
        DeleteMilestone = 24,
        AssignGroupForProject = 25,
        ReleaseGroupForProject = 26
    }
}

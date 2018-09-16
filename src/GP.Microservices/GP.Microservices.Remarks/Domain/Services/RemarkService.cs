using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Commands;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Remarks.Data;
using GP.Microservices.Remarks.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CommentStatus = GP.Microservices.Remarks.Domain.Models.CommentStatus;

namespace GP.Microservices.Remarks.Domain.Services
{
    public class RemarkService : IRemarkService
    {
        private readonly RemarksContext _context;

        public RemarkService(RemarksContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Remark>> BrowseAsync(BrowseRemarks query)
        {
            return await _context.Remarks.ToListAsync();
        }

        public async Task<Remark> GetAsync(Guid id)
        {
            return await _context
                .Remarks
                .Include(x => x.Category)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Remark> CreateAsync(CreateRemark command)
        {
            var remark = new Remark(command.Name, 
                command.Description, command.Latitude, command.Longitude,
                command.CategoryId, command.UserId);

            var entry = await _context.Remarks.AddAsync(remark);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<Remark> ResolveAsync(ResolveRemark command)
        {
            var remark = await _context.Remarks.SingleOrDefaultAsync(x => x.Id == command.RemarkId);
            remark.Status = RemarkStatus.Resolved;

            await _context.SaveChangesAsync();

            return remark;
        }

        public async Task<Remark> CancelAsync(CancelRemark command)
        {
            var remark = await _context.Remarks.SingleOrDefaultAsync(x => x.Id == command.RemarkId);
            remark.Status = RemarkStatus.Canceled;

            await _context.SaveChangesAsync();

            return remark;
        }

        public async Task<Remark> DeleteAsync(DeleteRemark command)
        {
            var remark = await _context.Remarks.SingleOrDefaultAsync(x => x.Id == command.RemarkId);
            remark.Status = RemarkStatus.Deleted;

            await _context.SaveChangesAsync();

            return remark;
        }

        public async Task<IEnumerable<Activity>> GetActivitiesAsync(Guid remarkId)
        {
            var activities = await _context.Activities
                .Where(x => x.RemarkId == remarkId)
                .ToListAsync();

            return activities;
        }

        public async Task<Activity> AddActivityAsync(AddActivity command)
        {
            var activity = new Activity(command.RemarkId,
                command.ActivityTypeId, command.UserId);

            var entry = await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(Guid remarkId)
        {
            var comments = await _context.Comments
                .Where(x => x.RemarkId == remarkId)
                .ToListAsync();

            foreach (var comment in comments)
            {
                if (comment.Status == CommentStatus.Deleted)
                    comment.Text = string.Empty;
            }

            return comments;
        }

        public async Task<Comment> AddCommentAsync(AddComment command)
        {
            var comment = new Comment(command.Text,
                command.RemarkId, command.UserId);

            var entry = await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<Comment> RemoveCommentAsync(RemoveComment command)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(x => x.Id == command.CommentId);

            comment.Status = CommentStatus.Deleted;
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<IEnumerable<Image>> GetImagessAsync(Guid remarkId)
        {
            var images = await _context.Images
                .Where(x => x.RemarkId == remarkId)
                .ToListAsync();

            return images;
        }

        public async Task<Image> AddImageAsync(AddImage command)
        {
            //todo: store image and get url
            await Task.Delay(new Random().Next(900, 1000));
            var image = new Image(command.Name, "url", command.RemarkId, command.UserId, command.ActivityId);
            var entry = await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task<Image> RemoveImageAsync(RemoveImage command)
        {
            var image = await _context.Images.SingleOrDefaultAsync(x => x.Id == command.ImageId);
            var entry = _context.Remove(image);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
    }
}
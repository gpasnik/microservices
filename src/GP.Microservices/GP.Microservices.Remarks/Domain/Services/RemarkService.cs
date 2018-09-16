using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GP.Microservices.Common.Messages.Remarks.Commands;
using GP.Microservices.Common.Messages.Remarks.Events;
using GP.Microservices.Common.Messages.Remarks.Queries;
using GP.Microservices.Remarks.Data;
using GP.Microservices.Remarks.Domain.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using CommentStatus = GP.Microservices.Remarks.Domain.Models.CommentStatus;
using RemarkCanceled = GP.Microservices.Common.Messages.Remarks.Events.RemarkCanceled;

namespace GP.Microservices.Remarks.Domain.Services
{
    public class RemarkService : IRemarkService
    {
        private readonly RemarksContext _context;
        private readonly IBus _bus;

        public RemarkService(
            RemarksContext context,
            IBus bus)
        {
            _context = context;
            _bus = bus;
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

            await _bus.Publish(new RemarkCreated
            {
                RemarkId = entry.Entity.Id
            });

            return entry.Entity;
        }

        public async Task<Remark> ResolveAsync(ResolveRemark command)
        {
            var remark = await _context.Remarks.SingleOrDefaultAsync(x => x.Id == command.RemarkId);
            remark.Status = RemarkStatus.Resolved;

            await _context.SaveChangesAsync();

            await _bus.Publish(new RemarkResolved
            {
                RemarkId = remark.Id
            });

            return remark;
        }

        public async Task<Remark> CancelAsync(CancelRemark command)
        {
            var remark = await _context.Remarks.SingleOrDefaultAsync(x => x.Id == command.RemarkId);
            remark.Status = RemarkStatus.Canceled;

            await _context.SaveChangesAsync();

            await _bus.Publish(new RemarkCanceled
            {
                RemarkId = remark.Id
            });

            return remark;
        }

        public async Task<Remark> DeleteAsync(DeleteRemark command)
        {
            var remark = await _context.Remarks.SingleOrDefaultAsync(x => x.Id == command.RemarkId);
            remark.Status = RemarkStatus.Deleted;

            await _context.SaveChangesAsync();

            await _bus.Publish(new RemarkDeleted
            {
                RemarkId = remark.Id
            });

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

            await _bus.Publish(new ActivityAdded
            {
                RemarkId = entry.Entity.RemarkId,
                ActivityId = entry.Entity.Id
            });

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

            await _bus.Publish(new CommentAdded
            {
                RemarkId = entry.Entity.RemarkId,
                CommentId = entry.Entity.Id
            });

            return entry.Entity;
        }

        public async Task<Comment> RemoveCommentAsync(RemoveComment command)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(x => x.Id == command.CommentId);

            comment.Status = CommentStatus.Deleted;
            await _context.SaveChangesAsync();

            await _bus.Publish(new CommentAdded
            {
                RemarkId = comment.RemarkId,
                CommentId = comment.Id
            });

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

            await _bus.Publish(new ImageAdded
            {
                RemarkId = entry.Entity.RemarkId,
                ImageId = entry.Entity.Id
            });

            return entry.Entity;
        }

        public async Task<Image> RemoveImageAsync(RemoveImage command)
        {
            var image = await _context.Images.SingleOrDefaultAsync(x => x.Id == command.ImageId);
            var entry = _context.Remove(image);
            await _context.SaveChangesAsync();

            await _bus.Publish(new ImageRemoved
            {
                RemarkId = entry.Entity.RemarkId,
                ImageId = entry.Entity.Id
            });

            return entry.Entity;
        }
    }
}
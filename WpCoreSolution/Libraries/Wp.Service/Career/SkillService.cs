﻿using System.Collections.Generic;
using System.Linq;
using Wp.Core;
using Wp.Core.Domain.Career;
using Wp.Data;
using Wp.Services.Events;

namespace Wp.Services.Career
{
   

    public class SkillService : EntityService<Skill>, ISkillService
    {
        private IBaseRepository<Skill> _skillRepo;
        private IBaseRepository<SkillItem> _skillItemRepo;

        public SkillService(IUnitOfWork unitOfWork, IBaseRepository<Skill> skillRepo, IBaseRepository<SkillItem> skillItemRepo, IEventPublisher eventPublisher)
        :base(unitOfWork, skillRepo, eventPublisher)
        {
            _unitOfWork = unitOfWork;
            this._skillRepo = skillRepo;
            this._skillItemRepo = skillItemRepo;
        }

        #region Ski
        public IList<Skill> GetAll(int ResumeId)
        {
            return _skillRepo.Table.Where(x => x.ResumeId == ResumeId).ToList();
        }       

        #endregion

        #region Item

        public IList<SkillItem> GetSkillItemsBySkillId(int skillId)
        {
            return _skillItemRepo.Table.Where(x => x.SkillId == skillId).ToList();
        }

        public SkillItem GetSkillItemById(int id)
        {
            return _skillItemRepo.GetById(id);
        }

        public void InsertSkillItem(SkillItem t)
        {
            _skillItemRepo.Add(t);
        }

        public void UpdateSkillItem(SkillItem t)
        {
            _unitOfWork.Complete();
        }

        public void DeleteSkillItem(SkillItem t)
        {
            _skillItemRepo.Remove(t);
            _unitOfWork.Complete();
        }

        #endregion
    }
}

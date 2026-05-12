using ProjetPOO.Model.Combat.Enums;
using ProjetPOO.Utilities.EntriesValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetPOO.Model.Combat
{
    public class EnemyInstance : Character
    {
        private int _templateId;
        private int _enemyRaceId;
        private string? _enemyName;

        public int TemplateId
        {
            get => _templateId;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _templateId = value;
                }
            }
        }

        public int EnemyRaceId
        {
            get => _enemyRaceId;
            private set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                {
                    _enemyRaceId = value;
                }
            }
        }

        public string? EnemyName
        {
            get => _enemyName;
            private set => _enemyName = value;
        }

        public EnemyInstance(Enemy template)
            : base(template.Name, template.MaxHp, template.Attack, template.Defense, template.Agility)
        {
            TemplateId = template.Id;
            EnemyName = template.EnemyName;
            EnemyRaceId = template.EnemyRaceId;
        }

    }
}

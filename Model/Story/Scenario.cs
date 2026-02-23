using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetPOO.Utilities.EntriesValidation;

namespace ProjetPOO.Model.Story
{
    public class Scenario
    {
        private const int MINIMUM_TITLE_LENGTH = 3;
        private const int MAX_TITLE_LENGTH = 200;
        private const int MINIMUM_DESCRIPTION_LENGTH = 30;
        private int _id;
        private string _title;
        private string _description;
        private int _startSceneId;
        private List<Scene> _scenes;



        public int Id
        {
            get => _id;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _id = value;
            }
        }
        public string Title
        {
            get => _title;
            set
            {
                if (ValidUtils.CheckEntryName(value, MINIMUM_TITLE_LENGTH, MAX_TITLE_LENGTH))
                    _title = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (ValidUtils.CheckEntryDescription(value, MINIMUM_DESCRIPTION_LENGTH))
                    _description = value;
            }
        }

        public int StartSceneId
        {
            get => _startSceneId;
            set
            {
                if (ValidUtils.CheckIfPositiveNumber(value))
                    _startSceneId = value;
            }
        }

        public IReadOnlyList<Scene> Scenes => _scenes.AsReadOnly();

        public Scenario()
        {
            _scenes = new List<Scene>();
        }

        public Scenario(int id, string title, string description, int startSceneId)
        {
            Id = id;
            Title = title;
            Description = description;
            StartSceneId = startSceneId;
            _scenes = new List<Scene>();
        }

        public Scene GetStartScene()
        {
            throw new System.NotImplementedException();
        }

        public Scene? GetSceneById(int sceneId)
        {
            throw new System.NotImplementedException();
        }

        public void AddScene(Scene scene)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveScene(int sceneId)
        {
             throw new System.NotImplementedException();
        }

        public bool Validate(out List<string> errors)
        {
            throw new System.NotImplementedException();
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{
    public class Territory : MonoBehaviour
    {
        public string Name;
        public Continent Continent;

        public List<Territory> NeighbourTerritories = new();
        public Player Owner;
        public int Troops { get; private set; }

        public Action OnStateChanged;
        public Action<Player,Player> OnOwnerChanged;
        public Action<int, int> OnTroopsChanged;
        

        public void Start()
        {
            SetupChecks();
        }


        private void SetupChecks()
        {
            if (!Name.Equals(name))
                Debug.LogError($"Territory name ({Name}) does not match GameObject name ({gameObject.name})");

            if (NeighbourTerritories.Count == 0)
                Debug.LogError("Territory " + Name + " has no neighbours");

            if (Continent == null)
                Debug.LogError("Territory " + Name + " has no continent");

            if (NeighbourTerritories.Contains(this))
                Debug.LogError("Territory " + Name + " has itself as neighbour");

            if (NeighbourTerritories.Count != NeighbourTerritories.Distinct().Count())
                Debug.LogError("Territory " + Name + " has duplicate neighbours");

            foreach (var neighbourTerritory in NeighbourTerritories)
            {
                if (!neighbourTerritory.NeighbourTerritories.Contains(this))
                {
                    Debug.LogError(
                        $"Territory {Name} has neighbour {neighbourTerritory.Name} that does not have {Name} as neighbour");
                }
            }
        }


        public void AddTroops(int amount)
        {
            var oldTroops = Troops;
            Troops += amount;
            OnTroopsChanged?.Invoke(oldTroops, Troops);
            OnStateChanged?.Invoke();
        }

        public void RemoveTroops(int amount)
        {
            var oldTroops = Troops;
            Troops -= amount;
            if (Troops < 0)
            {
                Debug.LogError($"Troops of {Name} went negative ({Troops})!");
                Troops = 0;
            }

            OnTroopsChanged?.Invoke(oldTroops, Troops);
            OnStateChanged?.Invoke();
        }

        public void SetOwner(Player newOwner)
        {
            Player oldOwner = Owner;
            if (oldOwner == newOwner)
            {
                Debug.LogWarning($"Tried to set owner of {Name} to {newOwner.Name}, but it already is owned by {newOwner.Name}!");
                return;
            }
            Owner = newOwner;
            if(oldOwner != null) oldOwner.RemoveTerritory(this);
            newOwner.AddTerritory(this);
            OnOwnerChanged?.Invoke(oldOwner, newOwner);
            OnStateChanged?.Invoke();
        }
        
        public void SetOwner(Player newOwner, int troops)
        {
            Troops = troops;
            SetOwner(newOwner);
        }
        
        public void SetTroops(int troops)
        {
            Troops = troops;
            OnStateChanged?.Invoke();
        }
        

        public IEnumerable<Territory> GetEnemyNeighbours()
        {
            return NeighbourTerritories.Where(neighbour => neighbour.Owner != Owner);
        }

        public IEnumerable<Territory> GetFriendlyNeighbours()
        {
            return NeighbourTerritories.Where(neighbour => neighbour.Owner == Owner);
        }


        public bool IsNeighbourOf(Territory other)
        {
            return NeighbourTerritories.Contains(other);
        }

        public bool IsNeighbourOf(Player player)
        {
            return NeighbourTerritories.Any(neighbour => neighbour.Owner == player);
        }

        public bool HasAnyNeighbourEnemy()
        {
            return NeighbourTerritories.Any(neighbour => neighbour.Owner != Owner);
        }

        public bool HasAnyNeighbourFriendly()
        {
            return NeighbourTerritories.Any(neighbour => neighbour.Owner == Owner);
        }

        public int GetAvailableTroops()
        {
            return Troops - 1;
        }


        public override string ToString()
        {
            return $"Territory {Name},  ({Troops} troops) owned by {Owner.Name}";
        }


        [MenuItem("CONTEXT/Territory/Auto Setup")]
        static void AutoSetup(MenuCommand command)
        {
            Territory territory = (Territory)command.context;
            territory.Name = territory.gameObject.name;
            territory.Continent = territory.transform.parent.GetComponent<Continent>();

            // load sprite from name
            var spriteRenderer = territory.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
                spriteRenderer = territory.gameObject.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                string continentName = territory.Continent.Name;
                string spritePath = $"Assets/Sprites/territories/cropped/{continentName}/{territory.Name}.png";
                // string spritePath = $"Assets/Sprites/territories/cropped/n_america/alaska.png";
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (sprite) 
                    spriteRenderer.sprite = sprite;
                else 
                    Debug.LogWarning($"Could not find sprite at {spritePath}");
            }

            PrefabUtility.RecordPrefabInstancePropertyModifications(territory);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        [MenuItem("CONTEXT/Territory/remove duplicated neighbours")]
        static void RemoveDuplicatedNeighbours(MenuCommand command)
        {
            Territory territory = (Territory)command.context;
            territory.NeighbourTerritories = territory.NeighbourTerritories.Distinct().ToList();
            PrefabUtility.RecordPrefabInstancePropertyModifications(territory);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        [MenuItem("CONTEXT/Territory/add territory to neighbours")]
        static void AddTerritoryToNeighbours(MenuCommand command)
        {
            Territory territory = (Territory)command.context;
            foreach (var neighbourTerritory in territory.NeighbourTerritories)
            {
                if (!neighbourTerritory.NeighbourTerritories.Contains(territory))
                {
                    neighbourTerritory.NeighbourTerritories.Add(territory);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(neighbourTerritory);
                }
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            foreach (var neighbour in NeighbourTerritories)
            {
                if (neighbour != null)
                    Gizmos.DrawLine(transform.position, neighbour.transform.position);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UIElements;

using Avidity;


public class PlaylistSelectorController : SelectorController
{
    public VisualElement root;

    public Label listName;
    public Label artistNames;
    public Label listKind;

    public Label listDuration;
    public Label listPlays;

    public Label listShard;
    public Label listCover;
    public Label listColour;


    void OnEnable()
    {
        this.root = this.ui.Q<VisualElement>("selector-playlist");

        this.listName    = this.root.Q<Label>("list-name");
        this.artistNames = this.root.Q<Label>("artist-names");
        this.listKind    = this.root.Q<Label>("list-kind");

        this.listDuration = this.root.Q<Label>("list-duration");
        this.listPlays    = this.root.Q<Label>("list-plays");

        this.listShard  = this.root.Q<Label>("list-shard");
        this.listCover  = this.root.Q<Label>("list-cover");
        this.listColour = this.root.Q<Label>("list-colour");
    }

    protected override void Start()
    {
        Exec.Scene.onSelectionChanged += OnSelectionChanged;
    }


    void OnSelectionChanged()
    {
        if (Exec.Scene.selectedEntityType != Bases.EntityType.Playlist) {
            this.root.style.display = DisplayStyle.None;
            return;
        }

        this.root.style.display = DisplayStyle.Flex;
        var list = Exec.Scene.selectedEntity as Playlist;
        RegenerateDetails(list);
    }

    void RegenerateDetails(Playlist list)
    {
        this.listName.text    = list.DisplayName();
        this.artistNames.text = list.DisplayArtistNames(shorter: true);
        this.listKind.text    = list.kind.text;

        this.listDuration.text = list.DisplayDuration();
        this.listPlays.text    = list.totalPlays.ToString();

        this.listShard.text = list.shard;
        SetLabelText(this.listCover, list.cover_file, "Auto");
        // SetLabelText(this.listColour, list.colour, "Default");
    }
}

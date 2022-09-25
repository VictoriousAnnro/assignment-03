namespace Assignment3.Entities;

public class TagRepository : ITagRepository
{
    public (Response Response, int TagId) Create(TagCreateDTO tag){
        var t = new Tag{
            Name = tag.Name,
            Id = 1 //hvordan får vi korrekt id??
        };

        return (Response.Created, 1);
    }
    public IReadOnlyCollection<TagDTO> ReadAll(){
        return new List<TagDTO>(){};

    }
    public TagDTO Read(int tagId){
        TagDTO l = new TagDTO(1, "le");
        return l;
    }
    public Response Update(TagUpdateDTO tag){
        return Response.Created;

    }
    public Response Delete(int tagId, bool force = false){
        return Response.Created;

    }
}

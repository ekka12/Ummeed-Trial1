namespace BLL;

using DAL;
using BOL;

public class IOManage{
    public List<User> GetAll(){
        CollectionContext ctx = new CollectionContext();
        var user= from us in ctx.User select us;
        return user.ToList();
    }

    public List<ChildSts> GetChild(){
        CollectionContext ctx = new CollectionContext();
        var child = from ch in ctx.ChildSts select ch;
        return child.ToList<ChildSts>();
    }

    public ChildSts GetChildByChkey(string chkey){
        CollectionContext ctx = new CollectionContext();
        var ch = from c in ctx.ChildSts where (c.Chkey == chkey) select c;
        ChildSts child = ch.FirstOrDefault<ChildSts>(c => (c.Chkey == chkey));
        return child;

    }

    public bool ChildReg(string chname,string password,string role,int uid){
        CollectionContext ctx = new CollectionContext();
        bool status= false;
        ctx.User.Add(new User{Uname=chname,Password=password,Role=role,Uid=uid,Uskey=uid});
        ctx.SaveChanges();
       
        status= true;

      return status;
    }

    public bool Update(int chid,string payment,string date){
        CollectionContext ctx = new CollectionContext();
        bool status = false; 
       var child = ctx.ChildSts.FirstOrDefault(ch=>ch.Chid==chid);
        child.Payment=payment;
        child.Date=date;
    //    ctx.ChildSts.Update(new ChildSts{Payment=payment,Date=date});
        ctx.SaveChanges();
        status =true;

        return status;
        
    }
    
    public bool RemoveUser(int chid){
        CollectionContext ctx = new CollectionContext();
        bool status= false;
        var user= ctx.User.FirstOrDefault(us=>us.Uid==chid);
        ctx.User.Remove(user);
        ctx.SaveChanges();
        Remove(chid);
        status=true;
        return status;
    }
    public bool Remove(int chid){
        CollectionContext ctx = new CollectionContext();
        bool status = false;
        var child = ctx.ChildSts.FirstOrDefault(ch=>ch.Chid==chid);
        ctx.ChildSts.Remove(child);
        ctx.SaveChanges();
        
        
        status = true;

        return status;

    }

    public bool ChildInx(string chname, string date,string password,string role,int id,string chkey){
        
        CollectionContext ctx = new CollectionContext();
        bool status = false;
        ctx.ChildSts.Add(new ChildSts{Chname=chname,Date=date,Chkey=chkey, Payment="0"});
        ctx.SaveChanges();
        ChildSts child=GetChildByChkey(chkey);
        int uid=child.Chid;
          
        ChildReg(chname,password,role,uid);
        status=true;
        
        

        return status;
    }
}
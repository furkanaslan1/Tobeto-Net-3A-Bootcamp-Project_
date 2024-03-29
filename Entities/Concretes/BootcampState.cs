﻿using Core.Entities;

namespace Entities.Concretes;

public class BootcampState : BaseEntity<int>
{
    public string Name { get; set; }

    public ICollection<Bootcamp> Bootcamps { get; set; }
   

    public BootcampState()
    {
        Bootcamps = new HashSet<Bootcamp>();
    }

    public BootcampState(int id,string name)
    {
        Id = id;
        Name = name;
    }
}
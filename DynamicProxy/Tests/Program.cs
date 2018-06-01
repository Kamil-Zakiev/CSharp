using System;
using Castle.DynamicProxy;
using ProxyTypes;
using Xunit;

namespace Tests
{
    public class Program
    {
        [Fact]
        public void IsFreezable_should_be_false_for_objects_created_with_ctor()
        {
            var nonFreezablePet = new Pet();
            Assert.False(Freezable.IsFreezable(nonFreezablePet));
        }
  
        [Fact]
        public void IsFreezable_should_be_true_for_objects_created_with_MakeFreezable()
        {
            var freezablePet = Freezable.MakeFreezable<Pet>();
            Assert.True(Freezable.IsFreezable(freezablePet));
        }
  
        [Fact]
        public void Freezable_should_work_normally()
        {
            var pet = Freezable.MakeFreezable<Pet>();
            pet.Age = 3;
            
            pet.Name = "Rex";
            pet.Age += pet.Name.Length;
            pet.ToString();
        }
  
        [Fact]
        public void Frozen_object_should_throw_ObjectFrozenException_when_trying_to_set_a_property()
        {
            var pet = Freezable.MakeFreezable<Pet>();
            pet.Age = 3;
  
            Freezable.Freeze(pet);

            Assert.Throws<ObjectFrozenException>(delegate { pet.Name = "This should throw"; });
        }
  
        [Fact]
        public void Frozen_object_should_not_throw_when_trying_to_read_it()
        {
            var pet = Freezable.MakeFreezable<Pet>();
            pet.Age = 3;
  
            Freezable.Freeze(pet);

            Assert.True(Record.Exception(() => { var age = pet.Age;}) == null);
            Assert.True(Record.Exception(() => { var name = pet.Name;}) == null);
            Assert.True(Record.Exception(() => { var tr = pet.ToString();}) == null);
        }
  
        [Fact]
        public void Freeze_nonFreezable_object_should_throw_NotFreezableObjectException()
        {
            var rex = new Pet();
            Assert.Throws<NotFreezableObjectException>(() => Freezable.Freeze(rex));
        }
        
        [Fact]
        public void DynProxyGetTarget_should_return_proxy_itself()
        {
            var pet = Freezable.MakeFreezable<Pet>();
            var hack = pet as IProxyTargetAccessor;
            Assert.NotNull(hack);
            Assert.Same(pet, hack.DynProxyGetTarget());
        }
        
        [Fact]
        public void Freezable_should_not_hold_any_reference_to_created_objects()
        {
            var pet = Freezable.MakeFreezable<Pet>();
            var petWeakReference = new WeakReference(pet, false);
            pet = null;
            GC.Collect();
            Assert.False(petWeakReference.IsAlive, "Object should have been collected");
        }
        
        [Fact]
        public void Frozen_object_should_not_intercept_getters()
        {
            var pet = Freezable.MakeFreezable<Pet>();

            pet.Age = 123;
            var age = pet.Age;
            Freezable.Freeze(pet);

            Assert.Equal(2, Freezable.GetCountOfInterceptorInvocations<CountingInterceptor>(pet));
            Assert.Equal(1, Freezable.GetCountOfInterceptorInvocations<FreezableInterceptor>(pet));
        }
        
        [Fact]
        public void CountCalls()
        {
            var pet = Freezable.MakeFreezable<Pet>();

            pet.Age = 123;
            pet.Age = 1243;
            pet.Age = 1223;
            var age = pet.Age;
            var age2 = pet.Age;
            pet = null;
            
            var pet2 = Freezable.MakeFreezable<Pet>();
            
            pet2.Age = 123;
            pet2.Age = 1243;
            pet2.Age = 1223;
            var age3 = pet2.Age;
            var age4 = pet2.Age;
        }
    }
}
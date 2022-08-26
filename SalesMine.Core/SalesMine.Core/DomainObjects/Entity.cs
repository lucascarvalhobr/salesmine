using SalesMine.Core.Messages;
using System;
using System.Collections.Generic;

namespace SalesMine.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        private List<Event> _notifications;
        public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AddEvent(Event evt)
        {
            _notifications = _notifications ?? new List<Event>();

            _notifications.Add(evt);
        }

        public void RemoveEvent(Event evt)
        {
            _notifications?.Remove(evt);
        }

        public void ClearEvents()
        {
            _notifications?.Clear();
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 721) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}

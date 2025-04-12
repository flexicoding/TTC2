/*
Defines a record that abstracts a single subject with its respective name, lesson count
(here either 2, 3 or 5), teachers that could possibly teach the subject and all students
that have selected this subject.

It must not be assumed that each student gets represented as a list of subjects that they
have chosen but rather all subjects represent the choices of all students of the entire
grade in question. This incorrect, yet intuitive, representation can be visualized as
following:

Student -> Selection(..., [SubjectAbstract1(...), Subject Abstract2(...), ...])

The correct representation would rather be visualized like this:

Selection -> [SubjectAbstract1(..., [Student1, Student2, ...]), SubjectAbstract2(...), ...]

The reason behind this seemingly strange representation is the fact that it is more
efficient to get all of the people that are affected by a single subject in this
representation than in the other one. As such, there should be less boiler-plate code
needed to access these people.
 */

using System.Collections.Frozen;

namespace TTC.CourseGen;

public sealed record SubjectAbstract(string Name, int LessonsCount, FrozenSet<string> PossibleTeachers,
    FrozenSet<string> SelectedStudents)
{

}

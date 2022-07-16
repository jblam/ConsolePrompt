// JB 2022-07-16
// Two things we can't properly assert:
// 1) We're correctly setting and unsetting the console colours --
//    we could implement a mock console, but the test would rely
//    on the correctness of the mock, which is itself not testable.
//    Similarly for invoking the Console.Write and WriteLine methods.
// 2) The ConsolePromptInterpolationHandler emits colour ranges which
//    do not overlap and are monotonic increasing in range -- this is
//    not testable because a failing test case is not writable. (For
//    example, I cannot write a format string which causes a negative
//    number of chars to be written to the buffer.)